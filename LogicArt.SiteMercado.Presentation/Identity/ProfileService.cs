using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using LogicArt.SiteMercado.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LogicArt.SiteMercado.Presentation.Identity
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager) => _userManager = userManager;

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.FindFirstValue("sub");
            var user = await _userManager.FindByIdAsync(subject);
            if (user == null)
            {
                return;
            }

            // for test purposes only
            var pictures = new[]
            {
                "https://cdn.discordapp.com/attachments/617528226683682878/816356732887629834/avatar-01.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356738579038288/avatar-02.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356738848129075/avatar-03.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356739099525241/avatar-04.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356741561581578/avatar-05.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356742581452811/avatar-06.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356744430878724/avatar-07.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356744862367754/avatar-08.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356747480399882/avatar-09.png",
                "https://cdn.discordapp.com/attachments/617528226683682878/816356749195608064/avatar-10.png"
            };
            var randomPictureIndex = new Random().Next(0, pictures.Length);
            var picture = pictures[randomPictureIndex];

            var claims = new List<Claim> {
                new("username", user.UserName),
                new("picture", picture)};
            context.IssuedClaims.AddRange(claims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
