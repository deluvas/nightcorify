using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nightcorify.Helpers;

namespace Nightcorify.Helpers
{
    public static class CryptoHelper
    {
        /// <summary>
        /// a-zA-Z0-9 with punctuation charset
        /// </summary>
        public static char[] AZ09PCharSet = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*$?&=!%"
            .ToArray();

        /// <summary>
        /// Computes SHA256 (managed) for the given string and returns a hex encoded string
        /// </summary>
        public static string ComputeSHA256( string content )
        {
            var hasher = new SHA256Managed();
            byte[] sha256 = hasher.ComputeHash( Encoding.UTF8.GetBytes( content ) );
            return sha256.ToHexString();
        }

        public static string ComputeSHA256( FileStream filestream )
        {
            using ( var buffered = new BufferedStream( filestream ) ) {
                using( var sha256 = new SHA256Managed() ) {
                    byte[] hash = sha256.ComputeHash( buffered );
                    return hash.ToHexString();
                }
            }
        }

        public static string ToHexString( this byte[] arr )
        {
            var builder = new StringBuilder();
            foreach ( byte @byte in arr ) {
                builder.Append( @byte.ToString( "x2" ) );
            }
            return builder.ToString();
        }

        /// <summary>
        /// Generates a random hexadecimal number
        /// </summary>
        /// <param name="size">Size in bytes. Returned string will be double this size.</param>
        public static string RandomHex( int size = 25 )
        {
            var salt = new byte[size];
            using ( var secureRandom = new RNGCryptoServiceProvider() ) {
                secureRandom.GetBytes( salt );
            }

            var hash = new StringBuilder();
            foreach ( byte @byte in salt ) {
                hash.Append( @byte.ToString( "x2" ) );
            }

            return hash.ToString();
        }

        /// <summary>
        /// Shamelessly :D copy pasted from 
        /// http://stackoverflow.com/questions/54991/generating-random-passwords
        /// </summary>
        public static string GenerateRandomPassword( int length, IList<char> charSet )
        {
            var characterArray = charSet.Distinct().ToArray();

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes( bytes );

            var result = new char[length];
            for ( int i = 0; i < length; i++ ) {
                ulong value = BitConverter.ToUInt64( bytes, i * 8 );
                result[i] = characterArray[value % (uint)characterArray.Length];
            }

            return new string( result );
        }
    }
}
