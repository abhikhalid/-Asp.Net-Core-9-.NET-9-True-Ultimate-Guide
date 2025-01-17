using Microsoft.AspNetCore.Mvc;
using ViewComponents_Example.Models;

namespace ViewComponents_Example.ViewComponents
{
    [ViewComponent] //this attribute is optional as long as the class name ends with "ViewComponent"
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //here we can write the logic to get data from database or any other source or calculation logic


            PersonGridModel model = new PersonGridModel()
            {
                GridTitle = "Person List",
                Persons = new List<Person>()
                {
                    new Person() { PersonName = "John Doe", JobTitle = "Software Developer" },
                    new Person() { PersonName = "Jane Doe", JobTitle = "Software Developer" },
                    new Person() { PersonName = "John Smith", JobTitle = "Software Developer" },
                    new Person() { PersonName = "Jane Smith", JobTitle = "Software Developer" }
                }
            };

            //invoked a partial view

            ViewBag.Grid = model; //passing the model to the partial view
            //the default location of partial view is Views/Shared/Components/Grid/Default.cshtml
            return View(); 
            //return View("Sample"); //do this if you change the partial view name instead of 'Default.cshtml' 
        }
    }
}
