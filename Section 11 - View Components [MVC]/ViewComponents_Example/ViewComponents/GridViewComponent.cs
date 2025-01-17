using Microsoft.AspNetCore.Mvc;

namespace ViewComponents_Example.ViewComponents
{
    [ViewComponent] //this attribute is optional as long as the class name ends with "ViewComponent"
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //here we can write the logic to get data from database or any other source or calculation logic


            //invoked a partial view
            //the default location of partial view is Views/Shared/Components/Grid/Default.cshtml
            return View(); 
            //return View("Sample"); //do this if you change the partial view name instead of 'Default.cshtml' 
        }
    }
}
