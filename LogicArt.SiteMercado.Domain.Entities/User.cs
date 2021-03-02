using System;
using LogicArt.Arch.Domain.Entities;
using LogicArt.Identity.Entities.Abstraction;

namespace LogicArt.SiteMercado.Domain.Entities
{
    public class User : Entity<Guid>, IUserWithPassword
    {

        private string _password;
        private string _userName;

        public string Password
        {
            get => _password;
            set => this.SetWithNotify(value, ref _password);
        }

        public string UserName
        {
            get => _userName;
            set => this.SetWithNotify(value, ref _userName);
        }

        public override string ToString() => this.UserName;

    }
}
