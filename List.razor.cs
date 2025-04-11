namespace AuthenticationUI.Pages.Library
{
    public partial class List
    {

        protected override async Task OnInitializedAsync()
        {
            books = await libraryService.GetBooks();
        }
    }
}
