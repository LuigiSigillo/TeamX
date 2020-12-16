using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Rg.Plugins.Popup.Services;
using TeamX.Database;
using TeamX.Models;
using TeamX.Services;
using TeamX.Utils;
using Xamarin.Forms;

namespace TeamX
{
    public partial class ProfileAboutPage : ContentPage , INotifyPropertyChanged
    {
        private User User;
        private ProfileService PS;
        private bool IsAdmin;
        private UserCategoryView selectedItem { get; set; }
        private ObservableCollection<UserCategoryView> userCats;
        public ObservableCollection<UserCategoryView> UserCats
        {
            get { return userCats; }
            set
            {
                userCats = value;
                OnPropertyChanged(nameof(userCats));
            }
        }
        private IUserCategoryService UKS = new UserCategoryService();
        private IUserContactService UCS =  new UserContactService();


        //TO-DELETE : THIS HAS BEEN SET ONLY 4 THE INTERFACE TESTS
        //--------------------------->
        public ProfileAboutPage()
        {
            InitializeComponent();
        }
        //---------------------------<
        public ProfileAboutPage(User user)
        {
            InitializeComponent();

            User = user;

            PreparePage();

        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void PreparePage()
        {
            IsAdmin = new PropertyService().GetUser().Id == User.Id; 
            user_name.Text = User.NickName;
            UN_button.Text = Generator.GenerateIniziali(User.NickName);
            EditBtn.IsVisible = IsAdmin;
            EditBtn.Source = ImageSource.FromResource("TeamX.Icons.useredit.png");


            MessagingCenter.Subscribe<CategoryDescriptionPopup, string>(this, "CategoryDescription", UpdateDescription);
            MessagingCenter.Subscribe<ProfileDetailPage, User>(this, "UserNameUpdated", NameUpdated);
            MessagingCenter.Subscribe<ProfileDetailPage>(this, "ContactsChanged", ContactsChanged);

            PS = new ProfileService(User.Id);
            PrepareCategoriesStack();
            PrepareContactsStack();
        }

        private void UpdateDescription(CategoryDescriptionPopup arg1, string arg2)
        {
            var desk = arg2;
            if (string.IsNullOrWhiteSpace(arg2))
            {
                desk = "No description available";
            }
            UserCats.Remove(selectedItem);
            selectedItem.Description = desk;
            UserCats.Add(selectedItem);
            new UserCategoryService().AddDescription(User.Id, selectedItem.CategoryId, arg2);
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (!IsAdmin)
            {
                catLV.SelectedItem = null;
                return;
            }

            var item = (UserCategoryView)e.SelectedItem;
            if (item != null)
            {
                selectedItem = item;
                PopupNavigation.Instance.PushAsync(new CategoryDescriptionPopup(item.Description));
                catLV.SelectedItem = null;
            }
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void NameUpdated(ProfileDetailPage arg1, User arg2)
        {
            User = arg2;
        }

        void ContactsChanged(ProfileDetailPage obj)
        {
            PrepareContactsStack();
        }

        void PrepareCategoriesStack()
        {
            var usercats = new ObservableCollection<UserCategoryView>();
            var maxCat = CategoryService.GetMaxCategory();
            for (int i = 1; i<=maxCat; i++)
            {
                var desk = UKS.GetDescription(User.Id, i);
                usercats.Add(new UserCategoryView(i, desk, IsAdmin));
            }
            UserCats = usercats;

            catLV.ItemsSource = UserCats;

        }

        void PrepareContactsStack()
        {
            ContactsStack.Children.Clear();

            var cntcs = UCS.GetContacts(User.Id);
            foreach(UserContact cntc in cntcs)
            {
                if (!string.Equals(cntc.Type, "email"))
                {
                    ImageButton btn = BuildContactButton(cntc.ContactUri, cntc.Type);
                    ContactsStack.Children.Add(btn);
                }
            }
            ContactsStack.Children.Add(BuildEmailButton(User.Email));

        }

        ImageButton BuildEmailButton(string email)
        {
            var btn = new ImageButton
            {
                Source = ImageSource.FromResource("TeamX.Icons.email.png"),
                BorderColor = Color.White,
                HeightRequest = 70,
                WidthRequest = 70, 
                CornerRadius = 10
                

            };
            btn.Pressed += (object sender, EventArgs e) => Device.OpenUri(new Uri("mailto:"+email));
            return btn;

        }

        ImageButton BuildContactButton(string uri, string type) 
        {
            var btn = new ImageButton
            {
                Source = ImageSource.FromResource("TeamX.Icons."+type+".png"),
                BorderColor = Color.White,
                HeightRequest = 70,
                WidthRequest = 70,
                CornerRadius = 10


            };
            btn.Pressed += (object sender, EventArgs e) => Device.OpenUri(new Uri(uri));
            return btn;


        }

        void Edit_Pressed(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ProfileDetailPage(User));
        }

    }
}
