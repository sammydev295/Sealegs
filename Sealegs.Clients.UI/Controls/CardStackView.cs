//
//  Copyright (c) 2016 MatchboxMobile
//  Licensed under The MIT License (MIT)
//  http://opensource.org/licenses/MIT
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//  IN THE SOFTWARE.
//
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    #region Class Extensions

    static class Extensions
    {
        public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> e, int n) =>
            n >= 0 ? e.Skip(n).Concat(e.Take(n)) : e.RotateRight(-n);

        public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> e, int n) =>
            e.Reverse().RotateLeft(n).Reverse();
    }

    #endregion 

    public class CardStackView : ContentView
    {
        #region Class Item

        public class Item
        {
            public string Name { get; set; }
            public string Photo { get; set; }
            public string Quantity { get; set; }
            public string Description { get; set; }
        };

        #endregion

        #region Constants & Statics

        // back card scale
        const float BackCardScale = 0.8f;

        // speed of the animations
        const int AnimLength = 250;

        // 180 / pi
        const float DegreesToRadians = 57.2957795f;

        // higher the number less the rotation effect
        const float CardRotationAdjuster = 0.3f;

        #endregion 

        #region Fields

        // distance a card must be moved to consider to be swiped off
        public int CardMoveDistance { get; set; }

        public bool? wasLastSwipeLeft { get; set; }

        // three cards
        const int NumCards = 3;
        CarouselCardView[] cards = new CarouselCardView[NumCards];

        int[] cvIndex = new int[NumCards] { 2, 0, 1 };

        // the card at the top of the stack
        int topCardIndex;

        // distance the card has been moved
        float cardDistance = 0;

        // the last items index added to the stack of the cards
        int itemIndex = 0;

        // Disable event handler switch
        bool ignoreTouch = false;

        #endregion 

        #region Actions & Events

        // called when a card is swiped left/right with the card index in the ItemSource
        public Action<int> SwipedRight = null;
        public Action<int> SwipedLeft = null;

        // Same as the swipedxxx actions, just the published event eqvivalents
        public event EventHandler<int> SwipeRight;
        public event EventHandler<int> SwipeLeft;

        #endregion

        #region Properties

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem", typeof(CardStackViewItem), typeof(CardStackView), null, BindingMode.OneWayToSource);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(CardStackView), null,
            propertyChanged: OnItemsSourcePropertyChanged);

        public IList<CardStackViewItem> ItemsSource
        {
            get
            {
                return (IList<CardStackViewItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
                itemIndex = 0;
            }
        }

        /// <summary>
        /// Current selected card
        /// </summary>
        public CardStackViewItem SelectedItem => ItemsSource[itemIndex == -1 ? 0 : itemIndex];

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CardStackView)bindable).Setup();
        }

        #endregion

        #region CTOR

        public CardStackView()
        {
            // create a stack of cards in this view
            RelativeLayout view = new RelativeLayout();

            for (int i = 0; i < NumCards; i++)
            {
                var card = new CarouselCardView();
                cards[i] = card;
                card.InputTransparent = true;
                card.IsVisible = false;

                view.Children.Add(
                    card,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width;
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height;
                    })
                );
            }

            this.BackgroundColor = Color.Black;
            this.Content = view;

            // Hook touch gesture
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);
        }

        #endregion

        #region Setup

        void Setup()
        {
            // set the top card
            topCardIndex = 0;

            // create a stack of cards
            var src = ItemsSource.ToArray();
            for (int i = 0; i < Math.Min(NumCards, src.Count()); i++)
            {
                if (itemIndex >= src.Count()) break;

                var card = cards[i];
                var srcItem = src[itemIndex];
                var srcUri = src[itemIndex].ImagePath;

                card.Name.Text = srcItem.Name;
                card.Quantity.Text = srcItem.Quantity;
                card.Description.Text = srcItem.Description;
                card.ImagePath.Source = ImageSource.FromUri(new Uri(srcItem.ImagePath));
                card.IsVisible = true;
                card.Scale = GetScale(i);
                card.RotateTo(0, 0);
                card.TranslateTo(0, -card.Y, 0);
                ((RelativeLayout)this.Content).LowerChild(card);

                itemIndex++;
            }

            // Drop index to facilitate proper loading as we swipe left dtarting from item 0
            itemIndex = 1;
            wasLastSwipeLeft = null;
        }

        #endregion

        #region Pan Touch Event Handler

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    HandleTouchStart();
                    break;
                case GestureStatus.Running:
                    HandleTouch((float)e.TotalX);
                    break;
                case GestureStatus.Completed:
                    HandleTouchEnd();
                    break;
            }
        }

        #endregion 

        #region HandleTouchStart

        // handle touch event begins
        public void HandleTouchStart()
        {
            cardDistance = 0;
        }

        #endregion 

        #region HandleTouch

        // handle the ongoing touch event as the card is moved
        public void HandleTouch(float diff_x)
        {
            if (ignoreTouch)
                return;

            // Swipe direction
            bool swipeLeft = diff_x < 0;

            // Can we move at all
            if (!IsThereNextCard(swipeLeft))
                return;

            var ci = GetCardInfo(swipeLeft);

            // If swipe left, scale the back card up and push the fore card all the way to the back
            if (ci.backCard.IsVisible && swipeLeft)
            {
                ((RelativeLayout)this.Content).LowerChild(ci.foreCard);
                ci.backCard.Scale = Math.Min(BackCardScale + Math.Abs((cardDistance / CardMoveDistance) * (1.0f - BackCardScale)), 1.0f);
            }

            // If swipe right, scale the fore card up and push the back card all the way to the back
            if (ci.foreCard.IsVisible && !swipeLeft)
            {
                ((RelativeLayout)this.Content).LowerChild(ci.backCard);
                ci.foreCard.Scale = Math.Min(BackCardScale + Math.Abs((cardDistance / CardMoveDistance) * (1.0f - BackCardScale)), 1.0f);
            }

            // rotate and move the top card
            if (ci.topCard.IsVisible)
            {
                // move the card
                ci.topCard.TranslationX = (diff_x);

                // calculate a angle for the card
                float rotationAngel = (float)(CardRotationAdjuster * Math.Min(diff_x / this.Width, 1.0f));
                ci.topCard.Rotation = rotationAngel * DegreesToRadians;

                // keep a record of how far its moved
                cardDistance = diff_x;
            }
        }

        #endregion 

        #region HandleTouchEnd

        // to handle the end of the touch event
        async public void HandleTouchEnd()
        {
            ignoreTouch = true;

            bool swipeLeft = cardDistance < 0;
            var ci = GetCardInfo(swipeLeft);

            // if the card was moved enough to be considered swiped off
            try
            {
                if (Math.Abs((int)cardDistance) > CardMoveDistance)
                {
                    // move off the screen
                    ci.topCard.TranslateTo(!swipeLeft ? this.Width : -this.Width, 0, AnimLength / 2, Easing.SpringOut);
                    ci.topCard.IsVisible = false;

                    if (SwipedRight != null && !swipeLeft)
                        SwipedRight(itemIndex);
                    else if (SwipedLeft != null)
                        SwipedLeft(itemIndex);

                    // Same as the swipedxxx actions, just the published event eqvivalents
                    EventHandler<int> handlerRight = SwipeRight;
                    EventHandler<int> handlerLeft = SwipeLeft;
                    if (handlerRight != null && !swipeLeft)
                        handlerRight(this, itemIndex);
                    else if (handlerLeft != null)
                        handlerLeft(this, itemIndex);

                    // show the next card
                    ShowNextCard(cardDistance, ci);

                    wasLastSwipeLeft = swipeLeft;

                    topCardIndex += swipeLeft ? +1 : -1;
                }
                else // put the card back in the center
                {
                    // center card
                    ci.topCard.TranslateTo((-ci.topCard.X), -ci.topCard.Y, AnimLength, Easing.SpringOut);
                    ci.topCard.RotateTo(0, AnimLength, Easing.SpringOut);
                    ci.topCard.IsVisible = true;

                    // scale the back card down
                    if (ci.backCard.IsVisible && swipeLeft)
                        ci.backCard.ScaleTo(BackCardScale, AnimLength, Easing.SpringOut);
                    else if (ci.foreCard.IsVisible)  // scale the fore card down
                        ci.foreCard.ScaleTo(BackCardScale, AnimLength, Easing.SpringOut);
                }
            }
            catch (Exception ex)
            {
            }

            ignoreTouch = false;
        }

        #endregion

        #region ShowNextCard

        // show the next card
        void ShowNextCard(float distanceMoved, CardInfo lci)
        {
            bool swipeLeft = distanceMoved < 0;
            CardInfo nci = new CardInfo();

            try
            {
                // Rotate the cards
                if (swipeLeft)
                    cvIndex = cvIndex.RotateLeft(1).ToArray();
                else
                    cvIndex = cvIndex.RotateRight(1).ToArray();

                nci = GetCardInfo(swipeLeft);

                // Push the fore & back cards to the stack bottom visually 
                ((RelativeLayout)this.Content).LowerChild(nci.backCard);
                ((RelativeLayout)this.Content).LowerChild(nci.foreCard);

                // Move old top card (which is the new back or fore card) back to its postion on stack
                lci.topCard.TranslateTo((lci.topCard.X), lci.topCard.Y, AnimLength, Easing.SpringOut);
                lci.topCard.Rotation = 0;
                lci.topCard.IsVisible = true;

                // Scale the correct old back or fore card down
                if (lci.backCard.IsVisible && swipeLeft)
                    lci.backCard.ScaleTo(BackCardScale, AnimLength, Easing.SpringOut);
                else if (lci.foreCard.IsVisible)
                    lci.foreCard.ScaleTo(BackCardScale, AnimLength, Easing.SpringOut);

                // Show new top card 
                ((RelativeLayout)this.Content).RaiseChild(nci.topCard);
                nci.topCard.IsVisible = true;
                nci.topCard.TranslateTo(0, nci.topCard.Y, AnimLength, Easing.SpringOut);
                nci.topCard.RotateTo(0, AnimLength, Easing.SpringOut);
                nci.topCard.ScaleTo(1, AnimLength, Easing.SpringOut);

                // Load next card
                if (SetNextLoadIndex(swipeLeft) == -1)
                    return;

                // load new data for back or fore card depending on swipe direction
                var loadCard = swipeLeft ? nci.backCard : nci.foreCard;
                loadCard.Name.Text = ItemsSource[itemIndex].Name;
                loadCard.Quantity.Text = ItemsSource[itemIndex].Quantity;
                loadCard.Description.Text = ItemsSource[itemIndex].Description;
                loadCard.ImagePath.Source = ImageSource.FromUri(new Uri(ItemsSource[itemIndex].ImagePath));
            }
            catch (Exception ex)
            {
            }
            finally
            {
                DumpCards("ShowNextCard", nci);
            }
        }

        #endregion

        #region GetCardInfo

        struct CardInfo
        {
            public int foreIndex;
            public CarouselCardView foreCard;
            public int topIndex;
            public CarouselCardView topCard;
            public int backIndex;
            public CarouselCardView backCard;
        };

        CardInfo GetCardInfo(bool swipeLeft)
        {
            return new CardInfo
            {
                foreIndex = 0,
                foreCard = cards[cvIndex[0]],
                topIndex = 1,
                topCard = cards[cvIndex[1]],
                backIndex = 2,
                backCard = cards[cvIndex[2]]
            };
        }

        #endregion

        #region DumpCards

        void DumpCards(string prefix, CardInfo ci)
        {
            Debug.WriteLine(prefix);

            Debug.WriteLine($"topCardIndex:{topCardIndex}, itemIndex:{itemIndex}");
            Debug.WriteLine($"ForeCard:{ci.foreCard.Name.Text}, IsVisible:{ci.foreCard.IsVisible}, X:{ci.foreCard.X}, Y:{ci.foreCard.Y}, Scale:{ci.foreCard.Scale}, Rotation:{ci.foreCard.Rotation}");
            Debug.WriteLine($"TopCard:{ci.topCard.Name.Text}, IsVisible:{ci.topCard.IsVisible}, X:{ci.topCard.X}, Y:{ci.topCard.Y}, Scale:{ci.topCard.Scale}, Rotation:{ci.topCard.Rotation}");
            Debug.WriteLine($"BackCard:{ci.backCard.Name.Text}, IsVisible:{ci.backCard.IsVisible}, X:{ci.backCard.X}, Y:{ci.backCard.Y}, Scale:{ci.backCard.Scale}, Rotation:{ci.backCard.Rotation}");
        }

        #endregion

        #region IsThereNextCard

        // Get the next item index to load from source
        bool IsThereNextCard(bool swipeLeft)
        {
            return swipeLeft ? topCardIndex < ItemsSource.Count - 1 : topCardIndex > 0;
        }

        #endregion

        #region SetNextLoadIndex

        // Next item to load from source
        int SetNextLoadIndex(bool swipeLeft)
        {
            if (swipeLeft && (wasLastSwipeLeft == null || wasLastSwipeLeft.Value))
                return itemIndex < ItemsSource.Count ? ++itemIndex : -1;
            else if (!swipeLeft && (wasLastSwipeLeft == null || !wasLastSwipeLeft.Value))
                return itemIndex > 0 ? --itemIndex : -1;
            else if (swipeLeft && (wasLastSwipeLeft == null || !wasLastSwipeLeft.Value)) // direction change from left to right
            {
                itemIndex = itemIndex + NumCards < ItemsSource.Count ? itemIndex + NumCards - 1 : itemIndex;
                return itemIndex < ItemsSource.Count ? itemIndex : -1;
            }

            // direction change from right to left
            itemIndex = itemIndex - NumCards + 1 > 0 ? itemIndex - NumCards : itemIndex;
            return itemIndex > 0 ? itemIndex : -1;
        }

        #endregion

        #region GetScale

        // helper to get the scale based on the card index position relative to the top card
        float GetScale(int index)
        {
            return (index == topCardIndex) ? 1.0f : BackCardScale;
        }

        #endregion
    }
}


