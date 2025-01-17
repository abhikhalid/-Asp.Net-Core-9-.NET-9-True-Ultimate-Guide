using Microsoft.AspNetCore.Mvc;
using ViewComponents_Example.Models;

namespace ViewComponents_Example.ViewComponents
{
    [ViewComponent] //this attribute is optional as long as the class name ends with "ViewComponent"
    public class GridViewComponent : ViewComponent
    {
        //public async Task<IViewComponentResult> InvokeAsync(int x, int y)
        public async Task<IViewComponentResult> InvokeAsync(PersonGridModel personGridModel)
        {
            //here we can write the logic to get data from database or any other source or calculation logic


            

            //invoked a partial view

            //ViewBag.Grid = personGridModel; //passing the model to the partial view
            //the default location of partial view is Views/Shared/Components/Grid/Default.cshtml


            //instead of sending 'personGridModel' object as ViewData, we can directly pass it to the View() method as shown below
            return View(personGridModel); 
            //return View("Sample"); //do this if you change the partial view name instead of 'Default.cshtml' 
        }
    }
}
