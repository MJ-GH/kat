using kat.Areas.Identity.Code;
using kat.Code;
using kat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace kat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Class1 _class1;
        private readonly HashingExample _hashingExample;
        private readonly IServiceProvider _serviceProvider;
        private readonly MyUserRoleHandler _myUserRoleHandler;
        private readonly IDataProtector _dataProtector;
        private readonly CryptExample _cryptExample;

        public HomeController(
            ILogger<HomeController> logger,
            Class1 class1,
            HashingExample hashingExample,
            IServiceProvider serviceProvider,
            MyUserRoleHandler myUserRoleHandler,
            IDataProtectionProvider dataProtector,
            CryptExample cryptExample)
        {
            _logger = logger;
            _class1 = class1;
            _hashingExample = hashingExample;
            _serviceProvider = serviceProvider;
            _myUserRoleHandler = myUserRoleHandler;
            _dataProtector = dataProtector.CreateProtector("veryUniqueHomeControllerKey");
            _cryptExample = cryptExample;
        }

        [Authorize("RequireAuthenticatedUser")]
        public IActionResult Index()
        {
            string txt = "Hello World";
            string txt2 = "Hello Martin";

            string myText = _class1.GetText();
            string myText2 = _class1.GetText2();

            string myHashedText = _hashingExample.GetHashedText_MD5(txt);

            string myBCryptHash = _hashingExample.GetHashedText_BCrypt(txt);
            string myBCryptSalt = _hashingExample.genSalt_BCrypt();

            string txtInput = "Hello World";
            bool verifyResult = _hashingExample.verifyWithBCrypt(txtInput, myBCryptHash);

            string myEncryptedText = _cryptExample.Encrypt(txt2, _dataProtector);
            string myDecryptedText = _cryptExample.Decrypt(myEncryptedText, _dataProtector);

            IndexModel myModel = new IndexModel() { Text1 = myBCryptSalt, Text2 = myBCryptHash, Text3 = verifyResult.ToString(), Text4 = myEncryptedText, Text5 = myDecryptedText };

            return View(model: myModel);
        }

        [Authorize(Policy = "RequireAdminUser")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
