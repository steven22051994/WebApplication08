# WebApplication08
Scool Project

Edit: skeleton-as-content (Fork @Chrissemann94): Drag Folders into the main project to check functionality

- Major Changes to Content-folder and _Layout.cshtml in Shared!!!!!
- Added site.js file in Scripts-folder and deleted everything related to bootstrap.
- Added Upload Image to Create/Edit
- Create and Edit are now POST instead of GET / Reason: ImageFile
- Splited the logic in CreatePost / Image-dependent and Product-dependent ( Createt GenerateProduct(); ) 
- Fixed Bugs depending ImageUpload if no Image was Added
- Fixed Bug depending not"null"-able variables for price
- Removed ImagePath property | Replaced with ImagePath() Method
- Tested functionality


TO-DO Chrissemann:
- Check Bundle-Config so Bootstrap does not get included when generating Solution
- Adapt Nav-Bar Max-Width and Burger-Button 
- Minor Changes to site.js


TO-DO Steven:
- Price-Validation @Create does not Accept ',' / View depends on RegularExpresion Regardless if its Active or not
- Image for Vendors 
- Relative Path's
- Update Errormessage-View with additional Information to the Error that Occurs