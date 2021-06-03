using System;
using System.Security.Cryptography;
using System.Text;

namespace User.Application.Security
{
    public class NavalCrypto
    {

        //reference https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae

        /// <summary>
        /// Generating 16 byte of salt.
        /// Use only Convert.ToBase64String to preserve byte array size.
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt(int byteSize = 16)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[byteSize]);
            return Convert.ToBase64String(salt);
        }


        /// <summary>
        /// Hashes the text with salting
        /// </summary>
        /// <param name="text"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashText(string text, string salt)
        {
            byte[] salts = Convert.FromBase64String(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(text, salts, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashbytes = new byte[36];
            Array.Copy(salts, 0, hashbytes, 0, 16);
            Array.Copy(hash, 0, hashbytes, 16, 20);
            string SaltedHashedPassword = Convert.ToBase64String(hashbytes);
            string orgsalt = System.Text.Encoding.UTF8.GetString(salts);
            Console.WriteLine("salt in encodeing = " + orgsalt);
            return SaltedHashedPassword;
        }


        /// <summary>
        /// Extracts salt from hashed password assuming the first 16 byte is salt and the remaining 20 byte is original text. 
        /// Validates whether the originAL text byte is same as provided.
        /// </summary>
        /// <param name="HashedText"></param>
        /// <param name="TextToMatch"></param>
        /// <returns></returns>
        public static bool MatchPasswordNotConsideringSalt(string HashedText, string TextToMatch)
        {
            byte[] salt = new byte[16];
            byte[] hashbytes = Convert.FromBase64String(HashedText);
            Array.Copy(hashbytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(TextToMatch, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            Console.WriteLine("salt in encodeing = " + System.Text.Encoding.UTF8.GetString(salt));
            bool HashMatch = true;

            for (int i = 0; i < 20; i++)
                if (hashbytes[i + 16] != hash[i])
                    HashMatch = false;

            return HashMatch;
        }


        public static string DESEncrypt(string plaintText,string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plaintText);
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DESDecrypt(string ciperText,string key)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(ciperText);


            //Get your key from config file to open the lock!
            //string key = ConfigurationManager.AppSettings["SecurityKey"];
            


            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
