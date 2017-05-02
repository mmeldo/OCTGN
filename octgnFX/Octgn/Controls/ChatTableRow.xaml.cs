﻿/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System.Linq;
using System.Reflection;
using log4net;

namespace Octgn.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;

    using Octgn.Annotations;
    using Octgn.Core;
    using Octgn.Library.Utils;

    using Skylabs.Lobby;

    /// <summary>
    /// Interaction logic for ChatTableRow
    /// </summary>
    public partial class ChatTableRow : TableRow, INotifyPropertyChanged, IDisposable
    {
        internal static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The user.
        /// </summary>
        private User user;

        /// <summary>
        /// The message.
        /// </summary>
        private string message;

        /// <summary>
        /// The message date.
        /// </summary>
        private DateTime messageDate;

        private bool enableGifs;

        private bool enableImages;

        private bool isHighlighted;

        private bool isLightTheme;

        public event MouseEventHandler OnMouseUsernameEnter;
        public event MouseEventHandler OnMouseUsernameLeave;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatTableRow"/> class.
        /// </summary>
        public ChatTableRow()
            : this(new User("NoUser"), Guid.NewGuid().ToString(), "TestMessage", DateTime.Now, LobbyMessageType.Standard)
        {

        }

        public ChatTableRow(User user, string id, string message, DateTime messageDate, LobbyMessageType messageType)
        {
            this.InitializeComponent();
            this.Id = id;
            this.User = user;
            this.Message = message;
            this.MessageDate = messageDate;
            this.MessageType = messageType;
            this.Unloaded += OnUnloaded;
            this.Loaded += OnLoaded;
            enableGifs = Prefs.EnableChatGifs;
            enableImages = Prefs.EnableChatImages;
        }

        private void UsernameParagraphOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            if (OnMouseUsernameLeave != null) OnMouseUsernameLeave(this, mouseEventArgs);
        }

        private void UsernameParagraphOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (OnMouseUsernameEnter != null) OnMouseUsernameEnter(this, mouseEventArgs);
        }

        private void OnLoaded(object sender, EventArgs eventArgs)
        {
            //this.Loaded -= this.OnLoaded;
            //this.User = user;
            //this.Message = message;
            //this.MessageDate = messageDate;
            this.UsernameParagraph.MouseEnter += UsernameParagraphOnMouseEnter;
            this.UsernameParagraph.MouseLeave += UsernameParagraphOnMouseLeave;
            Program.OnOptionsChanged += ProgramOnOnOptionsChanged;
            this.ProgramOnOnOptionsChanged();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Program.OnOptionsChanged -= this.ProgramOnOnOptionsChanged;
            this.UsernameParagraph.MouseEnter -= this.UsernameParagraphOnMouseEnter;
            this.UsernameParagraph.MouseLeave -= this.UsernameParagraphOnMouseLeave;
        }

        private void ProgramOnOnOptionsChanged()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(this.ProgramOnOnOptionsChanged));
                return;
            }
            this.IsLightTheme = Prefs.UseLightChat;
            enableGifs = Prefs.EnableChatGifs;
            enableImages = Prefs.EnableChatImages;
        }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public User User
        {
            get
            {
                return this.user;
            }

            set
            {
                if (this.user == value) return;
                this.user = value;
                OnPropertyChanged("User");
                //Dispatcher.BeginInvoke(new Action(() =>
                //                                      {
                //                                          (UsernameParagraph.Inlines.FirstInline as Run).Text = this.user.UserName;
                //                                          //UsernameColumn.Width = GridLength.Auto;
                //                                          //UsernameColumn.Width = new GridLength(GetUsernameWidth());
                //                                      }));
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                Dispatcher.BeginInvoke(new Action(this.ConstructMessage));
            }
        }

        /// <summary>
        /// Gets or sets the message date.
        /// </summary>
        public DateTime MessageDate
        {
            get
            {
                return this.messageDate;
            }

            set
            {
                this.messageDate = value;
                Dispatcher.BeginInvoke(new Action(() =>
                                                      {
                                                          TimeParagraph.Inlines.Clear();
                                                          TimeParagraph.Inlines.Add(new Run(this.messageDate.ToShortTimeString()));
                                                      }));
            }
        }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public LobbyMessageType MessageType { get; set; }

        public string Id { get; set; }

        public bool IsHighlighted
        {
            get
            {
                return this.isHighlighted;
            }
            set
            {
                if (value.Equals(this.isHighlighted))
                {
                    return;
                }
                this.isHighlighted = value;
                this.OnPropertyChanged("IsHighlighted");
            }
        }

        public bool IsLightTheme
        {
            get
            {
                return this.isLightTheme;
            }
            set
            {
                if (value.Equals(this.isLightTheme))
                {
                    return;
                }
                this.isLightTheme = value;
                this.OnPropertyChanged("IsLightTheme");
            }
        }

        /// <summary>
        /// The get username width.
        /// </summary>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        private double GetUsernameWidth()
        {
            var f = new FormattedText(
                this.User.UserName,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                12,
                Brushes.Black);
            f.SetFontWeight(FontWeights.Bold);
            return f.Width + 10;
        }

        private static Brush almostBlack = new SolidColorBrush(Color.FromRgb(2, 2, 2));
        /// <summary>
        /// The construct message and takes care of hyperlinks and other visual fluff.
        /// </summary>
        private void ConstructMessage()
        {
            const string Urlsplit = "===OMGURL===";
            MessageParagraph.Inlines.Clear();
            var urledmess = Regex.Replace(this.message, RegexPatterns.Urlrx, Urlsplit + "$0" + Urlsplit);
            var urlSplits = urledmess.Split(new string[1] { Urlsplit }, StringSplitOptions.None);
            foreach (var urlChunk in urlSplits)
            {
                if (Regex.IsMatch(urlChunk, RegexPatterns.Urlrx))
                {
                    System.Uri uri;
                    bool gotIt;
                    if (!System.Uri.TryCreate(urlChunk, UriKind.Absolute, out uri))
                    {
                        gotIt = System.Uri.TryCreate("http://" + urlChunk, UriKind.Absolute, out uri);
                    }
                    else gotIt = true;

                    if (gotIt && uri != null)
                    {
                        if (!IsImageUrl(uri))
                        {
                            var hl = new Hyperlink(new Run(urlChunk)) { NavigateUri = uri };
                            hl.Foreground = Brushes.CornflowerBlue;
                            hl.RequestNavigate += this.RequestNavigate;
                            MessageParagraph.Inlines.Add(hl);
                            continue;
                        }
                        else
                        {
                            try
                            {
                                var bi = BitmapFrame.Create(uri);
                                var image = new Image
                                {
                                    Source = bi,
                                    Stretch = Stretch.Uniform,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                    Width = double.NaN,
                                    Height = double.NaN,
                                    StretchDirection = StretchDirection.DownOnly
                                };
                                var border = new Border
                                {
                                    MaxWidth = 300,
                                    MaxHeight = 500,
                                    Child = image,
                                    VerticalAlignment = VerticalAlignment.Top,
                                    HorizontalAlignment = HorizontalAlignment.Left
                                };
                                //if (enableGifs)
                                //{
                                //    ImageBehavior.SetAnimatedSource(image, bi);
                                //    ImageBehavior.SetRepeatBehavior(image, RepeatBehavior.Forever);
                                //}
                                var container = new InlineUIContainer(border);
                                var hl = new Hyperlink(container) { TextDecorations = null };
                                hl.NavigateUri = uri;


                                hl.RequestNavigate += this.RequestNavigate;

                                image.MouseUp += (a, b) =>
                                    {
                                        try
                                        {
                                            RequestNavigate(hl, new RequestNavigateEventArgs(hl.NavigateUri, null));
                                        }
                                        catch (Exception)
                                        {
                                        }

                                    };
                                MessageParagraph.Inlines.Add(hl);
                                continue;
                            }
                            catch
                            { }
                        }
                    }
                }

                var cardRegex = new Regex(@"({[^}]*})", RegexOptions.IgnoreCase);

                foreach (var s in cardRegex.Split(urlChunk))
                {
                    if (s.StartsWith("{c:") && s.EndsWith("}"))
                    {
                        ParseOutCards(s);
                        continue;
                    }

                    var sb = new StringBuilder();
                    var quoteCount = 0;
                    foreach (var c in s)
                    {
                        if (c == '`')
                        {
                            if (quoteCount == 0)
                            {
                                MessageParagraph.Inlines.Add(sb.ToString());
                                sb.Clear();
                            }
                            quoteCount++;
                        }
                        if (quoteCount == 2)
                        {
                            var str = sb.ToString().TrimStart('`');
                            var textBorder = new Border()
                            {
                                BorderBrush = almostBlack,
                                Background = Brushes.LightGray,
                                CornerRadius = new CornerRadius(2),
                                //Padding = new Thickness(10,1,10,1),
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                            var textBlock = new TextBlock()
                            {
                                Text = str,
                                FontWeight = FontWeights.Bold,
                                Foreground = almostBlack,
                                Background = null,
                                Margin = new Thickness(10, 0, 10, 0)
                            };
                            textBorder.Child = textBlock;
                            var block = new InlineUIContainer(textBorder);
                            MessageParagraph.Inlines.Add(block);
                            quoteCount = 0;
                            sb.Clear();
                        }
                        else
                            sb.Append(c);
                    }
                    MessageParagraph.Inlines.Add(sb.ToString());
                }
                //MessageParagraph.Inlines.Add(s);
            }

        }

        private void ParseOutCards(string cardChunk)
        {
            try
            {
                var parseRegex = new Regex(@"{c:(?<gameid>[0-9a-zA-Z\-]+){1}:(?<setid>[0-9a-zA-Z\-]+){1}:(?<cardid>[0-9a-zA-Z\-]+){1}:(?<name>[^}]+){1}}");

                if (parseRegex.IsMatch(cardChunk) == false)
                    return;

                var match = parseRegex.Match(cardChunk);
                if (match.Groups.Count != 5)
                    return;
                var gameId = Guid.Parse(match.Groups["gameid"].Value);
                var setId = Guid.Parse(match.Groups["setid"].Value);
                var cardId = Guid.Parse(match.Groups["cardid"].Value);
                var name = match.Groups["name"].Value;

                var card = Octgn.DataNew.DbContext.Get().Cards.FirstOrDefault(x => x.Id == cardId);

                if (card == null)
                {
                    //TODO Need some kind of hover over to at least say the game isn't installed,
                    // Optimal would be to have a fancy tooltip with an image and an install button.
                    MessageParagraph.Inlines.Add("?(" + name + ")");
                    //// See if we can find the game in our feeds
                    //var package = GameFeedManager.Get().GetPackages()
                    //    .Where(x => x.IsAbsoluteLatestVersion)
                    //    .Where(x=>x.Id.ToLower() == gameId.ToString().ToLower())
                    //    .OrderBy(x => x.Title)
                    //    .GroupBy(x => x.Id)
                    //    .Select(x => x.OrderByDescending(y => y.Version.Version).First())
                    //    .FirstOrDefault();
                }
				else
                {
                    var hl = new Hyperlink(new Run(card.Name));
                    hl.Foreground = Brushes.CornflowerBlue;
                    MessageParagraph.Inlines.Add(hl);
                }
            }
            catch (Exception e)
            {
                Log.Warn("[ParseOutCards] Error parsing " + cardChunk + " into card", e);
            }
            return;
        }

        bool IsImageUrl(System.Uri url)
        {
            if (!enableImages) return false;
            if ((SubscriptionModule.Get().IsSubscribed ?? false) == false) return false;
            try
            {
                var req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "HEAD";
                for (var i = 0; i < 3; i++)
                {
                    try
                    {
                        using (var resp = req.GetResponse())
                        {
                            return resp.ContentType.ToLower(CultureInfo.InvariantCulture).StartsWith("image/");
                        }
                    }
                    catch
                    {
                    }
                }

            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Happens on hyperlink click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The Request Navigate Event Arguments.
        /// </param>
        private void RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                var hl = (Hyperlink)sender;
                var navigateUri = hl.NavigateUri.ToString();
                Program.LaunchUrl(navigateUri);
                //Process.Start(new ProcessStartInfo(navigateUri));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            e.Handled = true;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (OnMouseUsernameEnter != null)
            {
                foreach (var d in OnMouseUsernameEnter.GetInvocationList())
                {
                    OnMouseUsernameEnter -= (MouseEventHandler)d;
                }
            }
            if (OnMouseUsernameLeave != null)
            {
                foreach (var d in OnMouseUsernameLeave.GetInvocationList())
                {
                    OnMouseUsernameLeave -= (MouseEventHandler)d;
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
