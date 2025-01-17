using Microsoft.AspNetCore.Mvc;

namespace ViewComponents_Example.ViewComponents
{
    [ViewComponent] //this attribute is optional as long as the class name ends with "ViewComponent"
    public class GridViewComponent : ViewComponent
    {
        Task<IViewComponentResult> InvokeAsync()
        {
            //invoked a partial view
            //the default location of partial view is Views/Shared/Components/Grid/Default.cshtml
            return View(); 
        }
    }
}
