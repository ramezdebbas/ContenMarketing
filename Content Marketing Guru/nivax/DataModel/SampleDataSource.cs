using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "Here are just a few of the Effective 2013 Marketing Strategies, New Ideas, and Old Ideas which can be integrated into the mix of your Top 2013 Small Business Marketing Strategies.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Trust in Advertising is Terrible",
                    "A recent report from market research giant Nielsen shows consumer trust in traditional media advertising has plummeted. Users don’t like, nor trust the sales messages.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nA recent report from market research giant Nielsen shows consumer trust in traditional media advertising has plummeted. Users don’t like, nor trust the sales messages \n\nThe survey found whopping 90% of OECD consumers said they trusted brand recommendations from friends or users they trusted online, while only 10% said they trusted messages from display advertising.\n\nContent Marketing represents a form of word-of-mouth marketing, whereby users consumer, engage and share your useful brand content. A strong content marketing strategy hits closer to the 90% trust level than any paid banner ad at the other end of the consumer trust scale.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Trust in Advertising is Terrible", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Content Marketing Guru" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Content Marketing Delivers Quality Lead Generation",
                     "Half the money I spend on advertising is wasted; the trouble is I don’t know which half,” said John Wanamaker. This problem may be solved with content marketing.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHalf the money I spend on advertising is wasted; the trouble is I don’t know which half,” said John Wanamaker. This problem may be solved with content marketing. In fact, Content marketing can convert 30% more organic traffic into high quality sales leads (source: MarketingSherpa). Great content marketing will attract potentially high value customers, and these customers will come back for more.\n\nThis compares to more traditional advertising methods, where media planners will buy an audience assuming that an ad message will be received by a target audience, and this also assumes the paid message will compel users to act. Content Marketing delivers on the strategy of ‘narrowcasting’ where brands focus on a smaller, core group of potential, high quality consumers.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Delivers Quality Lead Generation", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Content Marketing Fulfils a Desire for More Information",
                     "Back in 2010, when about 1/3 of consumers had Smartphones, users searched on average of 5.3 sources of information before making a purchase decision. By the start of 2012, when over half of consumers owned a smartphone, that figure had doubled to 10.4 sources of information.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nBack in 2010, when about 1/3 of consumers had Smartphones, users searched on average of 5.3 sources of information before making a purchase decision. By the start of 2012, when over half of consumers owned a smartphone, that figure had doubled to 10.4 sources of information.\n\nUser behaviour and technology is changing, consumers will now search out for information about your industry, before asking a real person or consulting other media.\n\nMarketers must rise to this challenge and build great content systems that engage, inspire, educate and inform users who seek.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Fulfils a Desire for More Information", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Content Marketing Improves SEO",
                     "Content Marketing will help you kill it on the SEO front, as search engines get smarter at delivering the right information to users, content marketing needs to be at the centre of any SEO strategy.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nContent Marketing will help you kill it on the SEO front, as search engines get smarter at delivering the right information to users, content marketing needs to be at the centre of any SEO strategy.\n\nGoogle for instance now heavily weights social sharing and link buzz, the more engaging and shareable your content is, the better your SEO rankings. Google will also heavily favour content it feels is relevant to its users, so your content has to be excellent.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Improves SEO", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Content Marketing Will Position Your Brand as a Leader",
                     "Producing great, useful, authentic content will position your brand as a leader in your category, pushing brand uplift and vital word-of-mouth recommendations.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nProducing great, useful, authentic content will position your brand as a leader in your category, pushing brand uplift and vital word-of-mouth recommendations.\n\nThe theory of content marketing is only just being realised by many brands, as CMOs move toward new ways of thinking, less advertising and sales based campaign spends, to more long term, cost effective and engaging content marketing focused strategy.\n\nAct now and start building your content platforms, inventory and engagement streams. Content Marketing is a marathon, not a sprint, it’s a long game and it will require many months of planning and strategic development.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Will Position Your Brand as a Leader", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Content Marketing Will Influence Consumer Decision Making",
                     "Content marketing can influence purchase intent and decision-making. Consumers are on average 70% of the way through the sales funnel before engaging directly with a brand.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nContent marketing can influence purchase intent and decision-making. Consumers are on average 70% of the way through the sales funnel before engaging directly with a brand.\n\nContent marketing allows you to influence decision makers well before they have made up their minds.\n\nFor example, a Roper Public Affairs study found 80% of business decision makers prefer to access company information via a series of articles over advertisements. 70% of decision makers said content marketing made them feel closer to the brand, and 60% said content marketing helped them make better purchasing decisions.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Will Influence Consumer Decision Making", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Consumers Will Love You",
                     "Great Content marketing is useful for the target audience, it aims to help, inform, inspire and entertain- Content marketing is never a sales pitch or highly disruptive.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nGreat Content marketing is useful for the target audience, it aims to help, inform, inspire and entertain- Content marketing is never a sales pitch or highly disruptive. The key is to produce great content to help your target audience achieve their goals and desires and to connect brand to individual in a meaningful, memorable way.\n\nContent Marketing thought leader Jay Baer calls this ‘Youtility’ – that is always placing the customer first, even if it means referring your customer to competitors or businesses that have nothing to do with your industry. Helping your current and potential customers first, and not even thinking of selling, will build trust, word-of-mouth and deliver high quality, long term customers, advocates and super fans.\n\nBeyond the customer first aspect, content marketing promises to turn brands into publishers, entertainers and informers. \n\nThrough Content Marketing brands like Coca-Cola and Proctor & Gamble hope to “own a disproportionate share of popular culture” through big content ideas.The man behind Coke’s '2020' content marketing strategy, Jonathan Mildenhall says, “we are successful when people are talking about our brands in the right way.”",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Consumers Will Love You", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Content Marketing Will Feed Your Native Advertising",
                     "Great content has to find a way to its target audience. One way content can be distributed is to seed content using paid advertising products called native advertising. This form of advertising is fast becoming the default replacement for untrustworthy banner advertising.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nGreat content has to find a way to its target audience. One way content can be distributed is to seed content using paid advertising products called native advertising.\n\nThis form of advertising is fast becoming the default replacement for untrustworthy banner advertising.\n\nNative advertising is placed in a newsfeed on social media or relevant partner websites as a suggested post.\n\nGood native advertising takes advantage of strong content marketing materials that are engaging, authentic, inspiring and educational and align with your audiences values and interests. These native ads drive your audience to your content marketing inventory and beyond.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Will Feed Your Native Advertising", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Content Marketing Delivers Strong ROI",
                     "Because good content will last a long time, your investment in content marketing will remain relevant for potentially many years. This is compared to a very expensive paid advertising campaign that may only last a few weeks, days or even minutes.s",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nBecause good content will last a long time, your investment in content marketing will remain relevant for potentially many years. This is compared to a very expensive paid advertising campaign that may only last a few weeks, days or even minutes. Moreover, great content marketing will also earn you significant ‘earned media’ whereby users and other media outlets may talk about and share your content to millions more users, potentially producing millions of dollars in free brand exposure.\n\nAlso, studies have shown that per dollar, content marketing produces 3 times more leads than SEM and costs 30% less. Source: Kapost & ELOQUA",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Content Marketing Delivers Strong ROI", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Content Marketing Guru" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Social Media Feeds On Your Content Marketing",
                     "Content forms the basis of your social media strategy, without great content, your social media platforms are worthless and people will not follow or engage with your brand on Twitter, Facebook or any other social network.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nContent forms the basis of your social media strategy, without great content, your social media platforms are worthless and people will not follow or engage with your brand on Twitter, Facebook or any other social network. Content marketing materials should always be produced with social sharing and engagement in mind and social media user behaviour should influence your content streams and strategy.\n\nContent Marketing using social media will also deliver on user generated content, linked to positive word-of-mouth sentiment, earned media and fan/advocate community building. The rise of the hashtag as a tool to index and contextualise conversations around brands presents limitless opportunities for brands to hold engaging conversations and emotional moments with customers. ",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Social Media Feeds On Your Content Marketing", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Content Marketing Guru" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
