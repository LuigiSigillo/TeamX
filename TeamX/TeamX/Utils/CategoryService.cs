using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TeamX.Utils
{

    /// <summary>
    /// Category service.
    /// </summary>
    /// NB: it is a static class. To use it you don't need to create an instance of it. 
    /// ----Example of usage : 
    /// ---- //some code
    /// ---- string ctg = CategoryService.GetCategory(id);
    /// ----//some code
    public static class CategoryService
    {
    
        private static Dictionary<int, string> Categories = new Dictionary<int, string>()
        {
            {1, "Gaming" },
            {2, "Coding" },
            {3, "Sport" },
            {4, "Music" },
            {5, "Cooking" },
            {6, "Study" },
            {7, "Other" }
        };

        
        private static Dictionary<int, ImageSource> Categories_Image = new Dictionary<int, ImageSource>()
        {
            {1, ImageSource.FromResource("TeamX.immagini.Gaming.jpeg") },
            {2, ImageSource.FromResource("TeamX.immagini.Coding1.jpg") },
            {3, ImageSource.FromResource("TeamX.immagini.jordan.jpg") },
            {4, ImageSource.FromResource("TeamX.immagini.music.jpg") },
            {5, ImageSource.FromResource("TeamX.immagini.cooking.jpg") },
            {6, ImageSource.FromResource("TeamX.immagini.study.jpg")},
            {7, ImageSource.FromResource("TeamX.immagini.other.jpg")}

        };


        private static Dictionary<int, Color> CategoriesColor = new Dictionary<int, Color>()
        {
            {1, Color.Turquoise },
            {2, Color.Coral},
            {3, Color.Plum},
            {4, Color.Plum},
            {5, Color.Plum},
            {6, Color.Yellow},
            {7, Color.White}

        };



        private static Dictionary<int, Color> Categories_TextColor = new Dictionary<int, Color>()
        {
            {1, Color.White },
            {2, Color.White},
            {3, Color.White},
            {4, Color.White},
            {5, Color.White},
            {6, Color.White},
            {7, Color.White}

        };


        public static List<string> GetCategories()
        {
            List<string> ctgs = new List<string>(Categories.Values);
            return ctgs;
        }

        /// <summary>
        /// Returns the max value from the categories' ids
        /// </summary>
        /// <returns>The max category.</returns>
        public static int GetMaxCategory()
        {
            return 6;
        }


        /// <summary>
        /// Takes the id of a category and returns the corresponding string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetCategory(int id)
        {
            return Categories[id];
        }

        public static Color GetColor(int id)
        {
            return CategoriesColor[id];

        }

        public static (ImageSource,Color) GetImage(int id)
        {
            return (Categories_Image[id],Categories_TextColor[id]);
        }
    }
}
