using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Maonot_Net.Models
{


        Account account = new Account(
          "my_cloud_name",
          "my_api_key",
          "my_api_secret");

        Cloudinary cloudinary = new Cloudinary(account);



}
