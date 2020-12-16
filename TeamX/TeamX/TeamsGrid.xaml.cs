using Refractored.XamForms.PullToRefresh;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamX.Models;
using TeamX.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamsGrid : ContentPage
    {
        private SearchService servizio = new SearchService();
        private NavigationBarView NavigationBar = new NavigationBarView();
        private int col ;
        private int riga;
        private ScrollView scroll;
        private StackLayout stack; // stack principale
        private Grid griglia;
        private ImageButton adbutton;
        private AbsoluteLayout abs;
        public ICommand RefreshCommand { get; set; }
        public TeamsGrid()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<SearchBarView, string>(this, "TextChanged", TextChanged);

            MessagingCenter.Subscribe<FiltersPage, string>(this, "LocationChanged", LocationChanged);
            MessagingCenter.Subscribe<FiltersPage, int>(this, "CategoryChanged", CategoryChanged);
            MessagingCenter.Subscribe<FiltersPage, int>(this, "DifficultyChanged", DifficultyChanged);
            MessagingCenter.Subscribe<FiltersPage,double>(this, "DistanceChanged",DistanceChanged);
            MessagingCenter.Subscribe<FiltersPage>(this, "Update", UpdateGrid);
            MessagingCenter.Subscribe<FiltersPage>(this, "Reset", Reset);
            
            MessagingCenter.Subscribe<TeamDetailPage, Team>(this, "TeamUpdated", UpdateTeam);
            MessagingCenter.Subscribe<TeamDetailPage, Team>(this, "TeamAdded", AddNewTeam);
            MessagingCenter.Subscribe<TeamDetailPage, string>(this, "TeamRemoved", RemoveTeam);
            MessagingCenter.Subscribe<ViewNotification>(this, "Ntf_removed", RefreshNavigationBar);
            RefreshCommand = new Command(ExecuteRefreshCommand_Handle);
            StackPreparation();
            abs = new AbsoluteLayout();
            abs.Children.Add(stack); // stack creato in StackPreparation
            abs.Children.Add(adbutton);
            

            NavigationPage.SetTitleView(this, NavigationBar);
            Content = abs;
        }


        private void RefreshNavigationBar(ViewNotification obj)
        {
            NavigationBar.Refresh();
        }
        private void UpdateGrid(FiltersPage obj)
        {
            OnRefresh();
        }

        private void Reset(FiltersPage obj)
        {
            servizio.d = false;
            servizio.c = false;
            servizio.l = false;
            servizio.n = false;
            servizio.dif = false;
            OnRefresh();
        }
        /// <summary>
        /// refreshare la grid dato che non ha un metodo già fatto 
        /// </summary>
        /// <param name="f"></param>
        private void OnRefresh(string f = null)
        {
            stack.Children.RemoveAt(1);
            servizio.FilterByName(f);
            var scrollin = new ScrollView { Content = Create_grid(servizio.GetTeams()), Padding=15 };
            var refreshView = new PullToRefreshLayout { Content = scrollin, RefreshColor=Color.FromHex("#00EA75") };
            refreshView.RefreshCommand = RefreshCommand;
            stack.Children.Add(refreshView);
           
        }

        // overloading se ho già la lista
        private void OnRefresh (IEnumerable <Team> collezione)
        {
            stack.Children.RemoveAt(1);
            var scrollin = new ScrollView { Content = Create_grid(collezione), Padding=15 };
            var refreshView = new PullToRefreshLayout { Content = scrollin, RefreshColor=Color.FromHex("#00EA75") };
            refreshView.RefreshCommand = RefreshCommand;
            stack.Children.Add(refreshView);
        }
        /********************************************************/
        void ExecuteRefreshCommand_Handle(Object obj)
        {
             if (IsBusy)
                return;
            IsBusy = true; 
            OnRefresh();
            IsBusy = false;
        }
        
        /********************************************************/

        /// <summary>
        /// filtra per location 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="location"></param>
        public void LocationChanged(FiltersPage arg1, string location)
        {
            servizio.FilterByLocation(location);
        }

        /// <summary>
        /// in fase di testing
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void DistanceChanged(FiltersPage arg1, double dist)
        {
            servizio.FilterByDistance(dist);
        }

        /// <summary>
        /// /* da implementare quando verranno aggiunte le categorie al team*/
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="Idcategoria"></param>
        public void CategoryChanged(FiltersPage arg1, int Idcategoria)
        {
            servizio.FilterByCategory(Idcategoria);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="Idcategoria"></param>
        public void DifficultyChanged(FiltersPage arg1, int IdDifficulty)
        {
            servizio.FilterByDifficulty(IdDifficulty);
        }

        /// <summary>
        /// Per motivi di ordine la creazione dei due stack uniti per search bar e scroll view avviene qui
        /// </summary>
        private void StackPreparation ()
        {
            griglia = Create_grid(servizio.GetTeams()); // creazione prima griglia
            /* creazione di stack ovvero il contenitore dove è tutta la page e
             * sottostack(horizontal) dove è la searchbar*/
            stack = new StackLayout() { /*BackgroundColor = Color.FromHex("#242F38")*/ };
            var stacksearchbar = new StackLayout
            { 
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.Center
            }; // stack dove è la searchbar
        
            stacksearchbar.Children.Add(new SearchBarView() { BackgroundColor=Color.Transparent});
            
            stack.Children.Add(stacksearchbar);

            scroll = new ScrollView() { Content = griglia, Padding=15 }; // implemento la scroll view solo per la grid
            var refreshView = new PullToRefreshLayout { Content = scroll, RefreshColor = Color.FromHex("#00EA75") };
            refreshView.RefreshCommand = RefreshCommand;
            stack.Children.Add(refreshView);
           
            AbsoluteLayout.SetLayoutBounds(stack, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(stack, AbsoluteLayoutFlags.All);

            /* adbutton */
            adbutton = new ImageButton() {BackgroundColor=Color.Transparent, 
                                            Source =ImageSource.FromResource("TeamX.Icons.addicon.png")};
            adbutton.Clicked += Addbutton_Clicked;
            
            AbsoluteLayout.SetLayoutBounds(adbutton, new Rectangle(1, 1, 70, 70));
            AbsoluteLayout.SetLayoutFlags(adbutton, AbsoluteLayoutFlags.PositionProportional);
            
        } 


        /// <summary>
        /// crea griglia con i team che gli vengono passati 
        /// </summary>
        /// <param name="teams"></param>
        /// <returns> ritorna una griglia con i team della lista passata</returns>
        private Grid Create_grid(IEnumerable<Team> teams)
        {
            
            col = 0; riga = 0;
            var griglianuova = new Grid { RowSpacing = 30,
                                          ColumnSpacing = 30,
                                          Padding=0};

            if (teams != null)
            {
                foreach (Team t in teams)
                {
                    griglianuova.Children.Add(new TeamsBox(t), col, riga);
                    col++;
                    if (col > 1)
                    {
                        col = 0;
                        riga++;
                    }
                }
                if (teams.Count() < 4 && teams.Count() > 0)
                {
                    while (griglianuova.Children.Count() <= 4)
                    {
                        griglianuova.Children.Add(new TeamsBox(teams.First()) { IsVisible = false }, col, riga);
                        col++;
                        if (col > 1)
                        {
                            col = 0;
                            riga++;
                        }
                    }
                }
            }
            return griglianuova;
        }


        /// <summary>
        /// ogni volta che cambia il testo nella search bar rimuovo il figlio 1 cioè la vecchia scroll grid
        /// e metto la nuova griglia filtrata
        /// </summary>
        /// <param name="source"></param>
        /// <param name="nuova"></param>
        public void TextChanged(SearchBarView source, string nuova)
        {
            OnRefresh(nuova);
        }

        async private void Addbutton_Clicked(object sender, EventArgs e)
        {
            // pusha la pagina di add del team di paolo 
            await Navigation.PushAsync(new TeamDetailPage(new Team()));

        }

        

        /// <summary>
        /// prendo i team nuovi tramite messagging center e li aggiungo alla griglia
        /// </summary>
        /// <param name="source"></param>
        /// <param name="t"></param>
        private void AddNewTeam (TeamDetailPage source, Team t)
        {
            var nuovalista = servizio.Add_team(t);
            OnRefresh(nuovalista);

        }


        private void UpdateTeam (TeamDetailPage source, Team t)
        {
            
            var nuovalista = servizio.Update_team(t);
            OnRefresh(nuovalista);
        }

        private void RemoveTeam(TeamDetailPage arg1, string teamID)
        {
            RemovingService.RemoveTeam(teamID);
            OnRefresh();
        }
    }
}