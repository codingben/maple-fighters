using System.IO;
using CommonCommunicationInterfaces;

namespace Registration.Common
{
    public struct RegisterRequestParameters : IParameters
    {
        public string Email;
        public string Password;
        public string FirstName;
        public string LastName;

        public RegisterRequestParameters(string email, string password, string firstName, string lastName)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Email);
            writer.Write(Password);
            writer.Write(FirstName);
            writer.Write(LastName);
        }

        public void Deserialize(BinaryReader reader)
        {
            Email = reader.ReadString();
            Password = reader.ReadString();
            FirstName = reader.ReadString();
            LastName = reader.ReadString();
        }
    }
}