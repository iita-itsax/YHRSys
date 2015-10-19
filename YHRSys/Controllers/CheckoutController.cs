using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YHRSys.Models;
using DropdownSelect.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Net;

namespace YHRSys.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        const string PromoCode = "YIIFSWA";
        SelectedValue selVal = new SelectedValue();

        private List<SelectListItem> States(string val)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select State--", Value = "" });
            items.Add(new SelectListItem() {Text="Abia", Value="Abia", Selected = selVal.checkForSelectedValue("Abia", val) });
            items.Add(new SelectListItem() { Text = "Adamawa", Value = "Adamawa", Selected = selVal.checkForSelectedValue("Adamawa", val) });
            items.Add(new SelectListItem() { Text = "Akwa Ibom", Value = "Akwa Ibom", Selected = selVal.checkForSelectedValue("Akwa Ibom", val) });
            items.Add(new SelectListItem() { Text = "Anambra", Value = "Anambra", Selected = selVal.checkForSelectedValue("Anambra", val) });
            items.Add(new SelectListItem() { Text = "Bauchi", Value = "Bauchi", Selected = selVal.checkForSelectedValue("Bauchi", val) });
            items.Add(new SelectListItem() { Text = "Bayelsa", Value = "Bayelsa", Selected = selVal.checkForSelectedValue("Bayelsa", val) });
            items.Add(new SelectListItem() { Text = "Benue", Value = "Benue", Selected = selVal.checkForSelectedValue("Benue", val) });
            items.Add(new SelectListItem() { Text = "Borno", Value = "Borno", Selected = selVal.checkForSelectedValue("Borno", val) });
            items.Add(new SelectListItem() { Text = "Cross River", Value = "Cross River", Selected = selVal.checkForSelectedValue("Cross River", val) });
            items.Add(new SelectListItem() { Text = "Delta", Value = "Delta", Selected = selVal.checkForSelectedValue("Delta", val) });
            items.Add(new SelectListItem() { Text = "Ebonyi", Value = "Ebonyi", Selected = selVal.checkForSelectedValue("Ebonyi", val) });
            items.Add(new SelectListItem() { Text = "Edo", Value = "Edo", Selected = selVal.checkForSelectedValue("Edo", val) });
            items.Add(new SelectListItem() { Text = "Ekiti", Value = "Ekiti", Selected = selVal.checkForSelectedValue("Ekiti", val) });
            items.Add(new SelectListItem() { Text = "Enugu", Value = "Enugu", Selected = selVal.checkForSelectedValue("Enugu", val) });
            items.Add(new SelectListItem() { Text = "Gombe", Value = "Gombe", Selected = selVal.checkForSelectedValue("Gombe", val) });
            items.Add(new SelectListItem() { Text = "Imo", Value = "Imo", Selected = selVal.checkForSelectedValue("Imo", val) });
            items.Add(new SelectListItem() { Text="Jigawa", Value="Jigawa", Selected = selVal.checkForSelectedValue("Jigawa", val)});
            items.Add(new SelectListItem() { Text="Kaduna", Value="Kaduna", Selected = selVal.checkForSelectedValue("Kaduna", val)});
            items.Add(new SelectListItem() { Text="Kano", Value="Kano", Selected = selVal.checkForSelectedValue("Kano", val)});
            items.Add(new SelectListItem() { Text="Katsina", Value="Katsina", Selected = selVal.checkForSelectedValue("Katsina", val)});
            items.Add(new SelectListItem() { Text="Kebbi", Value="Kebbi", Selected = selVal.checkForSelectedValue("Kebbi", val)});
            items.Add(new SelectListItem() { Text="Kogi", Value="Kogi", Selected = selVal.checkForSelectedValue("Kogi", val)});
            items.Add(new SelectListItem() { Text="Kwara", Value="Kwara", Selected = selVal.checkForSelectedValue("Kwara", val)});
            items.Add(new SelectListItem() { Text="Lagos", Value="Lagos", Selected = selVal.checkForSelectedValue("Lagos", val)});
            items.Add(new SelectListItem() { Text="Nassarawa", Value="Nassarawa", Selected = selVal.checkForSelectedValue("Nassarawa", val)});
            items.Add(new SelectListItem() { Text="Niger", Value="Niger", Selected = selVal.checkForSelectedValue("Niger", val)});
            items.Add(new SelectListItem() { Text="Ogun", Value="Ogun", Selected = selVal.checkForSelectedValue("Ogun", val)});
            items.Add(new SelectListItem() { Text="Ondo", Value="Ondo", Selected = selVal.checkForSelectedValue("Ondo", val)});
            items.Add(new SelectListItem() { Text="Osun", Value="Osun", Selected = selVal.checkForSelectedValue("Osun", val)});
            items.Add(new SelectListItem() { Text="Oyo", Value="Oyo", Selected = selVal.checkForSelectedValue("Oyo", val)});
            items.Add(new SelectListItem() { Text="Plateau", Value="Plateau", Selected = selVal.checkForSelectedValue("Plateau", val)});
            items.Add(new SelectListItem() { Text="Rivers", Value="Rivers", Selected = selVal.checkForSelectedValue("Rivers", val)});
            items.Add(new SelectListItem() { Text="Sokoto", Value="Sokoto", Selected = selVal.checkForSelectedValue("Sokoto", val)});
            items.Add(new SelectListItem() { Text="Taraba", Value="Taraba", Selected = selVal.checkForSelectedValue("Taraba", val)});
            items.Add(new SelectListItem() { Text="Yobe", Value="Yobe", Selected = selVal.checkForSelectedValue("Yobe", val)});
            items.Add(new SelectListItem() { Text="Zamfara", Value="Zamfara", Selected = selVal.checkForSelectedValue("Zamfara", val)});
            return items;
        }

        private List<SelectListItem> Countries(string val)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select Country--", Value = "" , Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Afghanistan", Value = "Afghanistan", Selected = selVal.checkForSelectedValue("Afghanistan", val) });
            items.Add(new SelectListItem() { Text = "Aland Islands", Value = "Aland Islands", Selected = selVal.checkForSelectedValue("Aland Islands", val) });
            items.Add(new SelectListItem() { Text = "Albania", Value = "Albania", Selected = selVal.checkForSelectedValue("Albania", val) });
            items.Add(new SelectListItem() { Text = "Algeria", Value = "Algeria", Selected = selVal.checkForSelectedValue("Algeria", val) });
            items.Add(new SelectListItem() { Text = "American Samoa", Value = "American Samoa", Selected = selVal.checkForSelectedValue("American Samoa", val) });
            items.Add(new SelectListItem() { Text = "Andorra", Value = "Andorra", Selected = selVal.checkForSelectedValue("Andorra", val) });
            items.Add(new SelectListItem() { Text = "Angola", Value = "Angola", Selected = selVal.checkForSelectedValue("Angola", val) });
            items.Add(new SelectListItem() { Text = "Anguilla", Value = "Anguilla", Selected = selVal.checkForSelectedValue("Anguilla", val) });
            items.Add(new SelectListItem() { Text = "Antarctica", Value = "Antarctica", Selected = selVal.checkForSelectedValue("Antarctica", val) });
            items.Add(new SelectListItem() { Text = "Antigua & Barbuda", Value = "Antigua & Barbuda", Selected = selVal.checkForSelectedValue("Antigua & Barbuda", val) });
            items.Add(new SelectListItem() { Text = "Argentina", Value = "Argentina", Selected = selVal.checkForSelectedValue("Argentina", val) });
            items.Add(new SelectListItem() { Text = "Armenia", Value = "Armenia", Selected = selVal.checkForSelectedValue("Armenia", val) });
            items.Add(new SelectListItem() { Text = "Aruba", Value = "Aruba", Selected = selVal.checkForSelectedValue("Aruba", val) });
            items.Add(new SelectListItem() { Text = "Asia", Value = "Asia", Selected = selVal.checkForSelectedValue("Asia", val) });
            items.Add(new SelectListItem() { Text = "Australia", Value = "Australia", Selected = selVal.checkForSelectedValue("Australia", val) });
            items.Add(new SelectListItem() { Text = "Austria", Value = "Austria", Selected = selVal.checkForSelectedValue("Austria", val) });
            items.Add(new SelectListItem() { Text = "Azerbaijan", Value = "Azerbaijan", Selected = selVal.checkForSelectedValue("Azerbaijan", val) });
            items.Add(new SelectListItem() { Text = "Bahamas, The", Value = "Bahamas, The", Selected = selVal.checkForSelectedValue("Bahamas, The", val) });
            items.Add(new SelectListItem() { Text = "Bahrain", Value = "Bahrain", Selected = selVal.checkForSelectedValue("Bahrain", val) });
            items.Add(new SelectListItem() { Text = "Bangladesh", Value = "Bangladesh", Selected = selVal.checkForSelectedValue("Bangladesh", val) });
            items.Add(new SelectListItem() { Text = "Barbados", Value = "Barbados", Selected = selVal.checkForSelectedValue("Barbados", val) });
            items.Add(new SelectListItem() { Text = "Belarus", Value = "Belarus", Selected = selVal.checkForSelectedValue("Belarus", val) });
            items.Add(new SelectListItem() { Text = "Belgium", Value = "Belgium", Selected = selVal.checkForSelectedValue("Belgium", val) });
            items.Add(new SelectListItem() { Text = "Belize", Value = "Belize", Selected = selVal.checkForSelectedValue("Belize", val) });
            items.Add(new SelectListItem() { Text = "Benin", Value = "Benin", Selected = selVal.checkForSelectedValue("Benin", val) });
            items.Add(new SelectListItem() { Text = "Bermuda", Value = "Bermuda", Selected = selVal.checkForSelectedValue("Bermuda", val) });
            items.Add(new SelectListItem() { Text = "Bhutan", Value = "Bhutan", Selected = selVal.checkForSelectedValue("Bhutan", val) });
            items.Add(new SelectListItem() { Text = "Bolivia", Value = "Bolivia", Selected = selVal.checkForSelectedValue("Bolivia", val) });
            items.Add(new SelectListItem() { Text = "Bonaire, St.Eustat, Saba", Value = "Bonaire, St.Eustat, Saba", Selected = selVal.checkForSelectedValue("Bonaire, St.Eustat, Saba", val) });
            items.Add(new SelectListItem() { Text = "Bosnia and Herzegovina", Value = "Bosnia and Herzegovina", Selected = selVal.checkForSelectedValue("Bosnia and Herzegovina", val) });
            items.Add(new SelectListItem() {Text="Botswana", Value="Botswana", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Bouvet Island", Value = "Bouvet Island", Selected = selVal.checkForSelectedValue("Bouvet Island", val) });
            items.Add(new SelectListItem() { Text = "Brazil", Value = "Brazil", Selected = selVal.checkForSelectedValue("Brazil", val) });
            items.Add(new SelectListItem() { Text = "British Indian Ocean T.", Value = "British Indian Ocean T.", Selected = selVal.checkForSelectedValue("British Indian Ocean T.", val) });
            items.Add(new SelectListItem() { Text = "British Virgin Islands", Value = "British Virgin Islands", Selected = selVal.checkForSelectedValue("British Virgin Islands", val) });
            items.Add(new SelectListItem() { Text = "Brunei Darussalam", Value = "Brunei Darussalam", Selected = selVal.checkForSelectedValue("Brunei Darussalam", val) });
            items.Add(new SelectListItem() { Text = "Bulgaria", Value = "Bulgaria", Selected = selVal.checkForSelectedValue("Bulgaria", val) });
            items.Add(new SelectListItem() { Text = "Burkina Faso", Value = "Burkina Faso", Selected = selVal.checkForSelectedValue("Burkina Faso", val) });
            items.Add(new SelectListItem() { Text = "Burundi", Value = "Burundi", Selected = selVal.checkForSelectedValue("Burundi", val) });
            items.Add(new SelectListItem() { Text = "Cabo Verde", Value = "Cabo Verde", Selected = selVal.checkForSelectedValue("Cabo Verde", val) });
            items.Add(new SelectListItem() { Text = "Cambodia", Value = "Cambodia", Selected = selVal.checkForSelectedValue("Cambodia", val) });
            items.Add(new SelectListItem() { Text = "Cameroon", Value = "Cameroon", Selected = selVal.checkForSelectedValue("Cameroon", val) });
            items.Add(new SelectListItem() { Text = "Canada", Value = "Canada", Selected = selVal.checkForSelectedValue("Canada", val) });
            items.Add(new SelectListItem() { Text = "Caribbean, the", Value = "Caribbean, the", Selected = selVal.checkForSelectedValue("Caribbean, the", val) });
            items.Add(new SelectListItem() { Text = "Cayman Islands", Value = "Cayman Islands", Selected = selVal.checkForSelectedValue("Cayman Islands", val) });
            items.Add(new SelectListItem() { Text = "Central African Republic", Value = "Central African Republic", Selected = selVal.checkForSelectedValue("Central African Republic", val) });
            items.Add(new SelectListItem() { Text = "Central America", Value = "Central America", Selected = selVal.checkForSelectedValue("Central America", val) });
            items.Add(new SelectListItem() { Text = "Chad", Value = "Chad", Selected = selVal.checkForSelectedValue("Chad", val) });
            items.Add(new SelectListItem() { Text = "Chile", Value = "Chile", Selected = selVal.checkForSelectedValue("Chile", val) });
            items.Add(new SelectListItem() { Text = "China", Value = "China", Selected = selVal.checkForSelectedValue("China", val) });
            items.Add(new SelectListItem() { Text = "Christmas Island", Value = "Christmas Island", Selected = selVal.checkForSelectedValue("Christmas Island", val) });
            items.Add(new SelectListItem() { Text = "Cocos (Keeling) Islands", Value = "Cocos (Keeling) Islands", Selected = selVal.checkForSelectedValue("Cocos (Keeling) Islands", val) });
            items.Add(new SelectListItem() { Text = "Colombia", Value = "Colombia", Selected = selVal.checkForSelectedValue("Colombia", val) });
            items.Add(new SelectListItem() { Text = "Comoros", Value = "Comoros", Selected = selVal.checkForSelectedValue("Comoros", val) });
            items.Add(new SelectListItem() { Text = "Congo", Value = "Congo", Selected = selVal.checkForSelectedValue("Congo", val) });
            items.Add(new SelectListItem() { Text = "Congo, Dem. Rep. of the", Value = "Congo, Dem. Rep. of the", Selected = selVal.checkForSelectedValue("Congo, Dem. Rep. of the", val) });
            items.Add(new SelectListItem() { Text = "Cook Islands", Value = "Cook Islands", Selected = selVal.checkForSelectedValue("Cook Islands", val) });
            items.Add(new SelectListItem() { Text = "Costa Rica", Value = "Costa Rica", Selected = selVal.checkForSelectedValue("Costa Rica", val) });
            items.Add(new SelectListItem() { Text = "Cote D'Ivoire", Value = "Cote D'Ivoire", Selected = selVal.checkForSelectedValue("Cote D'Ivoire", val) });
            items.Add(new SelectListItem() { Text = "Croatia", Value = "Croatia", Selected = selVal.checkForSelectedValue("Croatia", val) });
            items.Add(new SelectListItem() { Text = "Cuba", Value = "Cuba", Selected = selVal.checkForSelectedValue("Cuba", val) });
            items.Add(new SelectListItem() { Text = "Curaçao", Value = "Curaçao", Selected = selVal.checkForSelectedValue("Curaçao", val) });
            items.Add(new SelectListItem() { Text = "Cyprus", Value = "Cyprus", Selected = selVal.checkForSelectedValue("Cyprus", val) });
            items.Add(new SelectListItem() { Text = "Czech Republic", Value = "Czech Republic", Selected = selVal.checkForSelectedValue("Czech Republic", val) });
            items.Add(new SelectListItem() { Text = "Denmark", Value = "Denmark", Selected = selVal.checkForSelectedValue("Denmark", val) });
            items.Add(new SelectListItem() { Text = "Djibouti", Value = "Djibouti", Selected = selVal.checkForSelectedValue("Djibouti", val) });
            items.Add(new SelectListItem() { Text = "Dominica", Value = "Dominica", Selected = selVal.checkForSelectedValue("Dominica", val) });
            items.Add(new SelectListItem() { Text = "Dominican Republic", Value = "Dominican Republic", Selected = selVal.checkForSelectedValue("Dominican Republic", val) });
            items.Add(new SelectListItem() { Text = "East Timor (Timor-Leste)", Value = "East Timor (Timor-Leste)", Selected = selVal.checkForSelectedValue("East Timor (Timor-Leste)", val) });
            items.Add(new SelectListItem() { Text = "Ecuador", Value = "Ecuador", Selected = selVal.checkForSelectedValue("Ecuador", val) });
            items.Add(new SelectListItem() { Text = "Egypt", Value = "Egypt", Selected = selVal.checkForSelectedValue("Egypt", val) });
            items.Add(new SelectListItem() { Text = "El Salvador", Value = "El Salvador", Selected = selVal.checkForSelectedValue("El Salvador", val) });
            items.Add(new SelectListItem() { Text = "Equatorial Guinea", Value = "Equatorial Guinea", Selected = selVal.checkForSelectedValue("Equatorial Guinea", val) });
            items.Add(new SelectListItem() { Text = "Eritrea", Value = "Eritrea", Selected = selVal.checkForSelectedValue("Eritrea", val) });
            items.Add(new SelectListItem() { Text = "Estonia", Value = "Estonia", Selected = selVal.checkForSelectedValue("Estonia", val) });
            items.Add(new SelectListItem() { Text = "Ethiopia", Value = "Ethiopia", Selected = selVal.checkForSelectedValue("Ethiopia", val) });
            //items.Add(new SelectListItem() {Text="Europe", Value="Europe", Selected = selVal.checkForSelectedValue("", val)});
            //items.Add(new SelectListItem() {Text="European Union", Value="European Union", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Falkland Is. (Malvinas)", Value = "Falkland Is. (Malvinas)", Selected = selVal.checkForSelectedValue("Falkland Is. (Malvinas)", val) });
            items.Add(new SelectListItem() { Text = "Faroe Islands", Value = "Faroe Islands", Selected = selVal.checkForSelectedValue("Faroe Islands", val) });
            items.Add(new SelectListItem() { Text = "Fiji", Value = "Fiji", Selected = selVal.checkForSelectedValue("Fiji", val) });
            items.Add(new SelectListItem() { Text = "Finland", Value = "Finland", Selected = selVal.checkForSelectedValue("Finland", val) });
            items.Add(new SelectListItem() { Text = "France", Value = "France", Selected = selVal.checkForSelectedValue("France", val) });
            items.Add(new SelectListItem() { Text = "French Guiana", Value = "French Guiana", Selected = selVal.checkForSelectedValue("French Guiana", val) });
            items.Add(new SelectListItem() { Text = "French Polynesia", Value = "French Polynesia", Selected = selVal.checkForSelectedValue("French Polynesia", val) });
            items.Add(new SelectListItem() { Text = "French Southern Terr.", Value = "French Southern Terr.", Selected = selVal.checkForSelectedValue("French Southern Terr.", val) });
            items.Add(new SelectListItem() { Text = "Gabon", Value = "Gabon", Selected = selVal.checkForSelectedValue("Gabon", val) });
            items.Add(new SelectListItem() { Text = "Gambia, the", Value = "Gambia, the", Selected = selVal.checkForSelectedValue("Gambia, the", val) });
            items.Add(new SelectListItem() { Text = "Georgia", Value = "Georgia", Selected = selVal.checkForSelectedValue("Georgia", val) });
            items.Add(new SelectListItem() { Text = "Germany", Value = "Germany", Selected = selVal.checkForSelectedValue("Germany", val) });
            items.Add(new SelectListItem() { Text = "Ghana", Value = "Ghana", Selected = selVal.checkForSelectedValue("Ghana", val) });
            items.Add(new SelectListItem() { Text = "Gibraltar", Value = "Gibraltar", Selected = selVal.checkForSelectedValue("Gibraltar", val) });
            items.Add(new SelectListItem() { Text = "Greece", Value = "Greece", Selected = selVal.checkForSelectedValue("Greece", val) });
            items.Add(new SelectListItem() { Text = "Greenland", Value = "Greenland", Selected = selVal.checkForSelectedValue("Greenland", val) });
            items.Add(new SelectListItem() { Text = "Grenada", Value = "Grenada", Selected = selVal.checkForSelectedValue("Grenada", val) });
            items.Add(new SelectListItem() { Text = "Guadeloupe", Value = "Guadeloupe", Selected = selVal.checkForSelectedValue("Guadeloupe", val) });
            items.Add(new SelectListItem() { Text = "Guam", Value = "Guam", Selected = selVal.checkForSelectedValue("Guam", val) });
            items.Add(new SelectListItem() { Text = "Guatemala", Value = "Guatemala", Selected = selVal.checkForSelectedValue("Guatemala", val) });
            items.Add(new SelectListItem() { Text = "Guernsey and Alderney", Value = "Guernsey and Alderney", Selected = selVal.checkForSelectedValue("Guernsey and Alderney", val) });
            items.Add(new SelectListItem() { Text = "Guiana, French", Value = "Guiana, French", Selected = selVal.checkForSelectedValue("Guiana, French", val) });
            items.Add(new SelectListItem() { Text = "Guinea", Value = "Guinea", Selected = selVal.checkForSelectedValue("Guinea", val) });
            items.Add(new SelectListItem() { Text = "Guinea-Bissau", Value = "Guinea-Bissau", Selected = selVal.checkForSelectedValue("Guinea-Bissau", val) });
            items.Add(new SelectListItem() { Text = "Guinea, Equatorial", Value = "Guinea, Equatorial", Selected = selVal.checkForSelectedValue("Guinea, Equatorial", val) });
            items.Add(new SelectListItem() { Text = "Guyana", Value = "Guyana", Selected = selVal.checkForSelectedValue("Guyana", val) });
            items.Add(new SelectListItem() { Text = "Haiti", Value = "Haiti", Selected = selVal.checkForSelectedValue("Haiti", val) });
            items.Add(new SelectListItem() { Text = "Heard & McDonald Is.", Value = "Heard & McDonald Is.", Selected = selVal.checkForSelectedValue("Heard & McDonald Is.", val) });
            //items.Add(new SelectListItem() { Text = "Holy See (Vatican)", Value = "Holy See (Vatican)", Selected = selVal.checkForSelectedValue("Holy See (Vatican)", val) });
            items.Add(new SelectListItem() { Text = "Honduras", Value = "Honduras", Selected = selVal.checkForSelectedValue("Honduras", val) });
            items.Add(new SelectListItem() { Text = "Hong Kong, (China)", Value = "Hong Kong, (China)", Selected = selVal.checkForSelectedValue("Hong Kong, (China)", val) });
            items.Add(new SelectListItem() { Text = "Hungary", Value = "Hungary", Selected = selVal.checkForSelectedValue("Hungary", val) });
            items.Add(new SelectListItem() { Text = "Iceland", Value = "Iceland", Selected = selVal.checkForSelectedValue("Iceland", val) });
            items.Add(new SelectListItem() {Text="India", Value="India", Selected = selVal.checkForSelectedValue("India", val)});
            items.Add(new SelectListItem() {Text="Indonesia", Value="Indonesia", Selected = selVal.checkForSelectedValue("Indonesia", val)});
            items.Add(new SelectListItem() { Text = "Iran, Islamic Republic of", Value = "Iran, Islamic Republic of", Selected = selVal.checkForSelectedValue("Iran, Islamic Republic of", val) });
            items.Add(new SelectListItem() {Text="Iraq", Value="Iraq", Selected = selVal.checkForSelectedValue("Iraq", val)});
            items.Add(new SelectListItem() { Text = "Ireland", Value = "Ireland", Selected = selVal.checkForSelectedValue("Ireland", val) });
            items.Add(new SelectListItem() {Text="Israel", Value="Israel", Selected = selVal.checkForSelectedValue("Israel", val)});
            items.Add(new SelectListItem() {Text="Italy", Value="Italy", Selected = selVal.checkForSelectedValue("Italy", val)});
            //items.Add(new SelectListItem() { Text = "Ivory Coast (Cote d'Ivoire)", Value = "Ivory Coast (Cote d'Ivoire)", Selected = selVal.checkForSelectedValue("Ivory Coast (Cote d'Ivoire)", val) });
            items.Add(new SelectListItem() { Text = "Jamaica", Value = "Jamaica", Selected = selVal.checkForSelectedValue("Jamaica", val) });
            items.Add(new SelectListItem() { Text = "Japan", Value = "Japan", Selected = selVal.checkForSelectedValue("Japan", val) });
            items.Add(new SelectListItem() {Text="Jersey", Value="Jersey", Selected = selVal.checkForSelectedValue("Jersey", val)});
            items.Add(new SelectListItem() {Text="Jordan", Value="Jordan", Selected = selVal.checkForSelectedValue("Jordan", val)});
            items.Add(new SelectListItem() {Text="Kazakhstan", Value="Kazakhstan", Selected = selVal.checkForSelectedValue("Kazakhstan", val)});
            items.Add(new SelectListItem() {Text="Kenya", Value="Kenya", Selected = selVal.checkForSelectedValue("Kenya", val)});
            items.Add(new SelectListItem() { Text = "Kiribati", Value = "Kiribati", Selected = selVal.checkForSelectedValue("Kiribati", val) });
            items.Add(new SelectListItem() { Text = "Korea Dem. People's Rep.", Value = "Korea Dem. People's Rep.", Selected = selVal.checkForSelectedValue("Korea Dem. People's Rep.", val) });
            items.Add(new SelectListItem() { Text = "Korea, (South) Republic of", Value = "Korea, (South) Republic of", Selected = selVal.checkForSelectedValue("Korea, (South) Republic of", val) });
            items.Add(new SelectListItem() {Text="Kosovo", Value="Kosovo", Selected = selVal.checkForSelectedValue("Kosovo", val)});
            items.Add(new SelectListItem() {Text="Kuwait", Value="Kuwait", Selected = selVal.checkForSelectedValue("Kuwait", val)});
            items.Add(new SelectListItem() { Text = "Kyrgyzstan", Value = "Kyrgyzstan", Selected = selVal.checkForSelectedValue("Kyrgyzstan", val) });
            items.Add(new SelectListItem() { Text = "Lao People's Dem. Rep.", Value = "Lao People's Dem. Rep.", Selected = selVal.checkForSelectedValue("Lao People's Dem. Rep.", val) });
            items.Add(new SelectListItem() { Text = "Latvia", Value = "Latvia", Selected = selVal.checkForSelectedValue("Latvia", val) });
            items.Add(new SelectListItem() { Text = "Lebanon", Value = "Lebanon", Selected = selVal.checkForSelectedValue("Lebanon", val) });
            items.Add(new SelectListItem() { Text = "Lesotho", Value = "Lesotho", Selected = selVal.checkForSelectedValue("Lesotho", val) });
            items.Add(new SelectListItem() { Text = "Liberia", Value = "Liberia", Selected = selVal.checkForSelectedValue("Liberia", val) });
            items.Add(new SelectListItem() { Text = "Libyan Arab Jamahiriya", Value = "Libyan Arab Jamahiriya", Selected = selVal.checkForSelectedValue("Libyan Arab Jamahiriya", val) });
            items.Add(new SelectListItem() { Text = "Liechtenstein", Value = "Liechtenstein", Selected = selVal.checkForSelectedValue("Liechtenstein", val) });
            items.Add(new SelectListItem() { Text = "Lithuania", Value = "Lothuania", Selected = selVal.checkForSelectedValue("Lothuania", val) });
            items.Add(new SelectListItem() { Text = "Luxembourg", Value = "Luxembourg", Selected = selVal.checkForSelectedValue("Luxembourg", val) });
            items.Add(new SelectListItem() { Text = "Macao, (China)", Value = "Macao, (China)", Selected = selVal.checkForSelectedValue("Macao, (China)", val) });
            items.Add(new SelectListItem() { Text = "Macedonia, TFYR", Value = "Macedonia, TFYR", Selected = selVal.checkForSelectedValue("Macedonia, TFYR", val) });
            items.Add(new SelectListItem() { Text = "Madagascar", Value = "Madagascar", Selected = selVal.checkForSelectedValue("Madagascar", val) });
            items.Add(new SelectListItem() { Text = "Malawi", Value = "Malawi", Selected = selVal.checkForSelectedValue("Malawi", val) });
            items.Add(new SelectListItem() { Text = "Malaysia", Value = "Malaysia", Selected = selVal.checkForSelectedValue("Malaysia", val) });
            items.Add(new SelectListItem() { Text = "Maldives", Value = "Maldives", Selected = selVal.checkForSelectedValue("Maldives", val) });
            items.Add(new SelectListItem() { Text = "Mali", Value = "Mali", Selected = selVal.checkForSelectedValue("Mali", val) });
            items.Add(new SelectListItem() { Text = "Malta", Value = "Malta", Selected = selVal.checkForSelectedValue("Malta", val) });
            items.Add(new SelectListItem() { Text = "Man, Isle of", Value = "Man, Isle of", Selected = selVal.checkForSelectedValue("Man, Isle of", val) });
            items.Add(new SelectListItem() {Text="Marshall Islands", Value="Marshall Islands", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Martinique (FR)", Value = "Martinique (FR)", Selected = selVal.checkForSelectedValue("Martinique (FR)", val) });
            items.Add(new SelectListItem() { Text = "Mauritania", Value = "Mauritania", Selected = selVal.checkForSelectedValue("Mauritania", val) });
            items.Add(new SelectListItem() { Text = "Mauritius", Value = "Mauritius", Selected = selVal.checkForSelectedValue("Mauritius", val) });
            items.Add(new SelectListItem() { Text = "Mayotte (FR)", Value = "Mayotte (FR)", Selected = selVal.checkForSelectedValue("Mayotte (FR)", val) });
            items.Add(new SelectListItem() {Text="Mexico", Value="Mexico", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Micronesia, Fed. States of", Value = "Micronesia, Fed. States of", Selected = selVal.checkForSelectedValue("Micronesia, Fed. States of", val) });
            //items.Add(new SelectListItem() {Text="Middle East", Value="Middle East", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() {Text="Moldova, Republic of", Value="Moldova, Republic of", Selected = selVal.checkForSelectedValue("Moldova, Republic of", val)});
            items.Add(new SelectListItem() { Text = "Monaco", Value = "Monaco", Selected = selVal.checkForSelectedValue("Monaco", val) });
            items.Add(new SelectListItem() { Text = "Mongolia", Value = "Mongolia", Selected = selVal.checkForSelectedValue("Mongolia", val) });
            items.Add(new SelectListItem() { Text = "Montenegro", Value = "Montenegro", Selected = selVal.checkForSelectedValue("Montenegro", val) });
            items.Add(new SelectListItem() { Text = "Montserrat", Value = "Montserrat", Selected = selVal.checkForSelectedValue("Montserrat", val) });
            items.Add(new SelectListItem() { Text = "Morocco", Value = "Morocco", Selected = selVal.checkForSelectedValue("Morocco", val) });
            items.Add(new SelectListItem() { Text = "Mozambique", Value = "Mozambique", Selected = selVal.checkForSelectedValue("Mozambique", val) });
            items.Add(new SelectListItem() { Text = "Myanmar (ex-Burma)", Value = "Myanmar (ex-Burma)", Selected = selVal.checkForSelectedValue("Myanmar (ex-Burma)", val) });
            items.Add(new SelectListItem() { Text = "Namibia", Value = "Namibia", Selected = selVal.checkForSelectedValue("Namibia", val) });
            items.Add(new SelectListItem() { Text = "Nauru", Value = "Nauru", Selected = selVal.checkForSelectedValue("Nauru", val) });
            items.Add(new SelectListItem() {Text="Nepal", Value="Nepal", Selected = selVal.checkForSelectedValue("Napal", val)});
            items.Add(new SelectListItem() { Text = "Netherlands", Value = "Netherlands", Selected = selVal.checkForSelectedValue("Netherlands", val) });
            items.Add(new SelectListItem() { Text = "Netherlands Antilles", Value = "Netherlands Antilles", Selected = selVal.checkForSelectedValue("Netherlands Antilles", val) });
            items.Add(new SelectListItem() { Text = "New Caledonia", Value = "New Caledonia", Selected = selVal.checkForSelectedValue("New Caledonia", val) });
            items.Add(new SelectListItem() { Text = "New Zealand", Value = "New Zealand", Selected = selVal.checkForSelectedValue("New Zealand", val) });
            items.Add(new SelectListItem() { Text = "Nicaragua", Value = "Nicaragua", Selected = selVal.checkForSelectedValue("Nicaragua", val) });
            items.Add(new SelectListItem() { Text = "Niger", Value = "Niger", Selected = selVal.checkForSelectedValue("Niger", val) });
            items.Add(new SelectListItem() { Text = "Nigeria", Value = "Nigeria", Selected = selVal.checkForSelectedValue("Nigeria", val) });
            items.Add(new SelectListItem() { Text = "Niue", Value = "Niue", Selected = selVal.checkForSelectedValue("Niue", val) });
            items.Add(new SelectListItem() { Text = "Norfolk Island", Value = "Norfolk Island", Selected = selVal.checkForSelectedValue("Norfolk Island", val) });
            //items.Add(new SelectListItem() {Text="North America", Value="North America", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Northern Mariana Islands", Value = "Northern Mariana Islands", Selected = selVal.checkForSelectedValue("Northern Mariana Islands", val) });
            items.Add(new SelectListItem() { Text = "Norway", Value = "Norway", Selected = selVal.checkForSelectedValue("Norway", val) });
            items.Add(new SelectListItem() { Text = "Oceania", Value = "Oceania", Selected = selVal.checkForSelectedValue("Oceania", val) });
            items.Add(new SelectListItem() { Text = "Oman", Value = "Oman", Selected = selVal.checkForSelectedValue("Oman", val) });
            items.Add(new SelectListItem() { Text = "Pakistan", Value = "Pakistan", Selected = selVal.checkForSelectedValue("Pakistan", val) });
            items.Add(new SelectListItem() { Text = "Palau", Value = "Palau", Selected = selVal.checkForSelectedValue("Palau", val) });
            items.Add(new SelectListItem() { Text = "Palestinian Territory", Value = "Palestinian Territory", Selected = selVal.checkForSelectedValue("Palestinian Territory", val) });
            items.Add(new SelectListItem() { Text = "Panama", Value = "Panama", Selected = selVal.checkForSelectedValue("Panama", val) });
            items.Add(new SelectListItem() { Text = "Papua New Guinea", Value = "Papua New Guinea", Selected = selVal.checkForSelectedValue("Papua New Guinea", val) });
            items.Add(new SelectListItem() {Text="Paraguay", Value="Paraguay", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "Peru", Value = "Peru", Selected = selVal.checkForSelectedValue("Peru", val) });
            items.Add(new SelectListItem() { Text = "Philippines", Value = "Philippines", Selected = selVal.checkForSelectedValue("Philippines", val) });
            items.Add(new SelectListItem() { Text = "Pitcairn Island", Value = "Pitcairn Island", Selected = selVal.checkForSelectedValue("Pitcairn Islan", val) });
            items.Add(new SelectListItem() { Text = "Poland", Value = "Poland", Selected = selVal.checkForSelectedValue("Poland", val) });
            items.Add(new SelectListItem() { Text = "Portugal", Value = "Portugal", Selected = selVal.checkForSelectedValue("Portugal", val) });
            items.Add(new SelectListItem() { Text = "Puerto Rico", Value = "Puerto Rico", Selected = selVal.checkForSelectedValue("Puerto Rico", val) });
            items.Add(new SelectListItem() { Text = "Qatar", Value = "Qatar", Selected = selVal.checkForSelectedValue("Qatar", val) });
            items.Add(new SelectListItem() { Text = "Reunion (FR)", Value = "Reunion (FR)", Selected = selVal.checkForSelectedValue("Reunion (FR)", val) });
            items.Add(new SelectListItem() { Text = "Romania", Value = "Romania", Selected = selVal.checkForSelectedValue("Romania", val) });
            items.Add(new SelectListItem() { Text = "Russia (Russian Fed.)", Value = "Russia (Russian Fed.)", Selected = selVal.checkForSelectedValue("Russia (Russian Fed.)", val) });
            items.Add(new SelectListItem() { Text = "Rwanda", Value = "Rwanda", Selected = selVal.checkForSelectedValue("Rwanda", val) });
            items.Add(new SelectListItem() { Text = "Sahara, Western", Value = "Sahara, Western", Selected = selVal.checkForSelectedValue("Sahara, Western", val) });
            items.Add(new SelectListItem() { Text = "Saint Barthelemy (FR)", Value = "Saint Barthelemy (FR)", Selected = selVal.checkForSelectedValue("Saint Barthelemy (FR)", val) });
            items.Add(new SelectListItem() { Text = "Saint Helena (UK)", Value = "Saint Helena (UK)", Selected = selVal.checkForSelectedValue("Saint Helena (UK)", val) });
            items.Add(new SelectListItem() { Text = "Saint Kitts and Nevis", Value = "Saint Kitts and Nevis", Selected = selVal.checkForSelectedValue("Saint Kitts and Nevis", val) });
            items.Add(new SelectListItem() { Text = "Saint Lucia", Value = "Saint Lucia", Selected = selVal.checkForSelectedValue("Saint Lucia", val) });
            items.Add(new SelectListItem() { Text = "Saint Martin (FR)", Value = "Saint Martin (FR)", Selected = selVal.checkForSelectedValue("Saint Martin (FR)", val) });
            items.Add(new SelectListItem() { Text = "S Pierre & Miquelon(FR)", Value = "S Pierre & Miquelon(FR)", Selected = selVal.checkForSelectedValue("S Pierre & Miquelon(FR)", val) });
            items.Add(new SelectListItem() { Text = "S Vincent & Grenadines", Value = "S Vincent & Grenadines", Selected = selVal.checkForSelectedValue("S Vincent & Grenadines", val) });
            items.Add(new SelectListItem() { Text = "Samoa", Value = "Samoa", Selected = selVal.checkForSelectedValue("Samoa", val) });
            items.Add(new SelectListItem() { Text = "San Marino", Value = "San Marino", Selected = selVal.checkForSelectedValue("San Marino", val) });
            items.Add(new SelectListItem() { Text = "Sao Tome and Principe", Value = "Sao Tome and Principe", Selected = selVal.checkForSelectedValue("Sao Tome and Principe", val) });
            items.Add(new SelectListItem() { Text = "Saudi Arabia", Value = "Saudi Arabia", Selected = selVal.checkForSelectedValue("Saudi Arabia", val) });
            items.Add(new SelectListItem() { Text = "Senegal", Value = "Senegal", Selected = selVal.checkForSelectedValue("Senegal", val) });
            items.Add(new SelectListItem() { Text = "Serbia", Value = "Serbia", Selected = selVal.checkForSelectedValue("Serbia", val) });
            items.Add(new SelectListItem() { Text = "Seychelles", Value = "Seychelles", Selected = selVal.checkForSelectedValue("Seychelles", val) });
            items.Add(new SelectListItem() { Text = "Sierra Leone", Value = "Sierra Leone", Selected = selVal.checkForSelectedValue("Sierra Leone", val) });
            items.Add(new SelectListItem() { Text = "Singapore", Value = "Singapore", Selected = selVal.checkForSelectedValue("Singapore", val) });
            items.Add(new SelectListItem() { Text = "Slovakia", Value = "Slovakia", Selected = selVal.checkForSelectedValue("Slovakia", val) });
            items.Add(new SelectListItem() { Text = "Slovenia", Value = "Slovenia", Selected = selVal.checkForSelectedValue("Slovenia", val) });
            items.Add(new SelectListItem() { Text = "Solomon Islands", Value = "Solomon Islands", Selected = selVal.checkForSelectedValue("Solomon Islands", val) });
            items.Add(new SelectListItem() { Text = "Somalia", Value = "Somalia", Selected = selVal.checkForSelectedValue("Somalia", val) });
            items.Add(new SelectListItem() { Text = "South Africa", Value = "South Africa", Selected = selVal.checkForSelectedValue("South Africa", val) });
            //items.Add(new SelectListItem() {Text="South America", Value="South America", Selected = selVal.checkForSelectedValue("", val)});
            items.Add(new SelectListItem() { Text = "S.George & S.Sandwich", Value = "S.George & S.Sandwich", Selected = selVal.checkForSelectedValue("S.George & S.Sandwich", val) });
            items.Add(new SelectListItem() { Text = "South Sudan", Value = "South Sudan", Selected = selVal.checkForSelectedValue("South Sudan", val) });
            items.Add(new SelectListItem() { Text = "Spain", Value = "Spain", Selected = selVal.checkForSelectedValue("Spain", val) });
            items.Add(new SelectListItem() { Text = "Sri Lanka (ex-Ceilan)", Value = "Sri Lanka (ex-Ceilan)", Selected = selVal.checkForSelectedValue("Sri Lanka (ex-Ceilan)", val) });
            items.Add(new SelectListItem() { Text = "Sudan", Value = "Sudan", Selected = selVal.checkForSelectedValue("Sudan", val) });
            items.Add(new SelectListItem() { Text = "Suriname", Value = "Suriname", Selected = selVal.checkForSelectedValue("Suriname", val) });
            items.Add(new SelectListItem() { Text = "Svalbard & Jan Mayen Is.", Value = "Svalbard & Jan Mayen Is.", Selected = selVal.checkForSelectedValue("Svalbard & Jan Mayen Is.", val) });
            items.Add(new SelectListItem() { Text = "Swaziland", Value = "Swaziland", Selected = selVal.checkForSelectedValue("Swaziland", val) });
            items.Add(new SelectListItem() { Text = "Sweden", Value = "Sweden", Selected = selVal.checkForSelectedValue("Sweden", val) });
            items.Add(new SelectListItem() { Text = "Switzerland", Value = "Switzerland", Selected = selVal.checkForSelectedValue("Switzerland", val) });
            items.Add(new SelectListItem() { Text = "Syrian Arab Republic", Value = "Syrian Arab Republic", Selected = selVal.checkForSelectedValue("Syrian Arab Republic", val) });
            items.Add(new SelectListItem() { Text = "Taiwan", Value = "Taiwan", Selected = selVal.checkForSelectedValue("Taiwan", val) });
            items.Add(new SelectListItem() { Text = "Tajikistan", Value = "Tajikistan", Selected = selVal.checkForSelectedValue("Tajikistan", val) });
            items.Add(new SelectListItem() { Text = "Tanzania, United Rep. of", Value = "Tanzania, United Rep. of", Selected = selVal.checkForSelectedValue("Tanzania, United Rep. of", val) });
            items.Add(new SelectListItem() { Text = "Thailand", Value = "Thailand", Selected = selVal.checkForSelectedValue("Thailand", val) });
            items.Add(new SelectListItem() { Text = "Timor-Leste (East Timor)", Value = "Timor-Leste (East Timor)", Selected = selVal.checkForSelectedValue("Timor-Leste (East Timor)", val) });
            items.Add(new SelectListItem() { Text = "Togo", Value = "Togo", Selected = selVal.checkForSelectedValue("Togo", val) });
            items.Add(new SelectListItem() { Text = "Tokelau", Value = "Tokelau", Selected = selVal.checkForSelectedValue("Tokelau", val) });
            items.Add(new SelectListItem() { Text = "Tonga", Value = "Tonga", Selected = selVal.checkForSelectedValue("Tonga", val) });
            items.Add(new SelectListItem() { Text = "Trinidad & Tobago", Value = "Trinidad & Tobago", Selected = selVal.checkForSelectedValue("Trinidad & Tobago", val) });
            items.Add(new SelectListItem() { Text = "Tunisia", Value = "Tunisia", Selected = selVal.checkForSelectedValue("Tunisia", val) });
            items.Add(new SelectListItem() { Text = "Turkey", Value = "Turkey", Selected = selVal.checkForSelectedValue("Turkey", val) });
            items.Add(new SelectListItem() { Text = "Turkmenistan", Value = "Turkmenistan", Selected = selVal.checkForSelectedValue("Turkmenistan", val) });
            items.Add(new SelectListItem() { Text = "Turks and Caicos Is.", Value = "Turks and Caicos Is.", Selected = selVal.checkForSelectedValue("Turks and Caicos Is.", val) });
            items.Add(new SelectListItem() { Text = "Tuvalu", Value = "Tuvalu", Selected = selVal.checkForSelectedValue("Tuvalu", val) });
            items.Add(new SelectListItem() { Text = "Uganda", Value = "Uganda", Selected = selVal.checkForSelectedValue("Uganda", val) });
            items.Add(new SelectListItem() { Text = "Ukraine", Value = "Ukraine", Selected = selVal.checkForSelectedValue("Ukraine", val) });
            items.Add(new SelectListItem() { Text = "United Arab Emirates", Value = "United Arab Emirates", Selected = selVal.checkForSelectedValue("United Arab Emirates", val) });
            items.Add(new SelectListItem() { Text = "United Kingdom", Value = "United Kingdom", Selected = selVal.checkForSelectedValue("United Kingdom", val) });
            items.Add(new SelectListItem() { Text = "United States of America", Value = "United States of America", Selected = selVal.checkForSelectedValue("United States of America", val) });
            items.Add(new SelectListItem() { Text = "US Minor Outlying Isl.", Value = "US Minor Outlying Isl", Selected = selVal.checkForSelectedValue("US Minor Outlying Isl", val) });
            items.Add(new SelectListItem() { Text = "Uruguay", Value = "Uruguay", Selected = selVal.checkForSelectedValue("Uruguay", val) });
            items.Add(new SelectListItem() { Text = "Uzbekistan", Value = "Uzbekistan", Selected = selVal.checkForSelectedValue("Uzbekistan", val) });
            items.Add(new SelectListItem() { Text = "Vanuatu", Value = "Vanuatu", Selected = selVal.checkForSelectedValue("Vanuatu", val) });
            items.Add(new SelectListItem() { Text = "Vatican (Holy See)", Value = "Vatican (Holy See)", Selected = selVal.checkForSelectedValue("Vatican (Holy See)", val) });
            items.Add(new SelectListItem() { Text = "Venezuela", Value = "Venezuela", Selected = selVal.checkForSelectedValue("Venezuela", val) });
            items.Add(new SelectListItem() { Text = "Viet Nam", Value = "Viet Nam", Selected = selVal.checkForSelectedValue("Viet Nam", val) });
            items.Add(new SelectListItem() { Text = "Virgin Islands, British", Value = "Virgin Islands, British", Selected = selVal.checkForSelectedValue("Virgin Islands, British", val) });
            items.Add(new SelectListItem() { Text = "Virgin Islands, U.S.", Value = "Virgin Islands, U.S.", Selected = selVal.checkForSelectedValue("Virgin Islands, British", val) });
            items.Add(new SelectListItem() { Text = "Wallis and Futuna", Value = "Wallis and Futuna", Selected = selVal.checkForSelectedValue("Wallis and Futuna", val) });
            items.Add(new SelectListItem() { Text = "Western Sahara", Value = "Western Sahara", Selected = selVal.checkForSelectedValue("Western Sahara", val) });
            items.Add(new SelectListItem() { Text = "Yemen", Value = "Yemen", Selected = selVal.checkForSelectedValue("Yemen", val) });
            items.Add(new SelectListItem() { Text = "Zambia", Value = "Zambia", Selected = selVal.checkForSelectedValue("Zambia", val) });
            items.Add(new SelectListItem() { Text = "Zimbabwe", Value = "Zimbabwe", Selected = selVal.checkForSelectedValue("Zimbabwe", val) });
            return items;
        }


        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            ViewBag.Country = Countries("Nigeria");
            ViewBag.State = States("");
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    if (values["StateTxt"].ToString() != "")
                    {
                        ViewBag.StateTxt = values["StateTxt"];
                        order.State = values["StateTxt"];
                    }
                    ViewBag.Country = Countries(values["Country"]);
                    ViewBag.State = States(values["State"]);

                    ModelState.AddModelError(string.Empty, "Promo Code must be provided!");
                    return View(order);
                
                }
                else if (values["StateTxt"].ToString() == "" && values["State"].ToString() == "")
                {
                    ViewBag.Country = Countries(values["Country"]);
                    ViewBag.State = States(values["State"]);
                    ModelState.AddModelError(string.Empty, "State origin must be supplied!");
                    return View(order);
                }
                else
                {
                    if (values["StateTxt"].ToString()!="")
                        order.State = values["StateTxt"];

                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId());

                    if (currentUser != null){
                        order.Username = currentUser.UserName;
                        order.createdBy = currentUser.UserName;
                    }else{
                        order.Username = User.Identity.Name;
                        order.createdBy = User.Identity.Name;
                    }

                    order.status = ORDERSTATUS.NEW.ToString();
                    order.OrderDate = DateTime.Now;
                    order.createdDate = DateTime.Now;

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();
                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                }
            }
            catch(Exception ex)
            {
                if (values["StateTxt"].ToString() != "")
                {
                    ViewBag.StateTxt = values["StateTxt"];
                    order.State = values["StateTxt"];
                }
                    
                //Invalid - redisplay with errors
                ViewBag.Country = Countries(values["Country"]);
                ViewBag.State = States(values["State"]);
                ModelState.AddModelError(string.Empty, "Error Occurred: " + ex.Message);

                return View(order);
            }
        }

        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            bool isValid = false;

            if (currentUser != null)
                isValid = storeDB.Orders.Any(o => o.OrderId == id && o.Username == currentUser.UserName);
            else
                isValid = storeDB.Orders.Any(o => o.OrderId == id && o.Username == User.Identity.Name);

            if (isValid)
            {
                string[] groups = new string[] { "SuperAdmins" };
                List<ApplicationUser> groupUsers = CheckGroupAffiliate.GetGroupUsers(groups);
                string body = "";

                var order = storeDB.Orders.Find(id);
                if (order != null)
                {
                    body = string.Format("<p>New order has been sent from <strong>{0}, {1}</strong></p><p>ADDRESS:<br/>===============<br/>Email: {2}<br/>Address: {3}<br/>City: {4}<br/>State: {5}<br/>Country: {6}<br/></p><p>Click <a href=\"http://yamhrponline.iita.org/Orders/Details/{7}\">HERE</a> to access the Order</p>", order.LastName, order.FirstName, order.Email, order.Address, order.City, order.State, order.Country, id);

                    if (order.OrderDetails.Count() > 0)
                    {
                        OrderDetailProcess oDP = new OrderDetailProcess();

                        body += oDP.BodyString(order);
                    }
                }
                else
                    body = string.Format("New order has been sent with ID: {0}", id);
               
                System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                System.Net.Configuration.MailSettingsSectionGroup mailSettings = (System.Net.Configuration.MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                if (mailSettings != null)
                {
                    int port = mailSettings.Smtp.Network.Port;
                    string host = mailSettings.Smtp.Network.Host;
                    string password = mailSettings.Smtp.Network.Password;
                    string username = mailSettings.Smtp.Network.UserName;
                    string from = mailSettings.Smtp.From;

                    if (groupUsers.Count > 0)
                    {
                        foreach (ApplicationUser user in groupUsers)
                        {
                            //Debug.WriteLine("SUPER ADMIN: " + user.Email);
                            //using(var message = new MailMessage("k.oraegbunam@cgiar.org", user.Email)){
                            using (var message = new MailMessage(from, user.Email))
                            {
                                message.Subject = "YHRSys New Order Notification";
                                message.Body = body;
                                message.IsBodyHtml = true;
                                using (SmtpClient client = new SmtpClient
                                {
                                    EnableSsl = false,
                                    Host = host,//"casarray.iita.cgiarad.org",
                                    Port = port,//25
                                    Credentials = new NetworkCredential(username, password)
                                })
                                {
                                    try
                                    {
                                        client.Send(message);
                                    }
                                    catch (Exception ex)
                                    {
                                        new LogWriter(message.Subject + ", ERROR: " + ex.Message, "OrderNotification");
                                    }
                                }
                            }
                        }
                    }
                }

                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}