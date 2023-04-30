using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public class PetitionNotificationService : BackgroundService
    {
        private readonly IPetitionRepository _petitionRepository;
        private readonly IUserRepository _userRepository;
        //private readonly INotificationService _notificationService;
        private readonly int _voteThreshold;
        private readonly IEmailService _emailService;
        public ILogger<PetitionNotificationService> _logger;
        
        public PetitionNotificationService( ILogger<PetitionNotificationService> logger, IPetitionRepository petitionRepository, IUserRepository userRepository, IEmailService emailService, IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;
            _petitionRepository = petitionRepository;
            _userRepository = userRepository;
           // _notificationService = notificationService;
          //  _voteThreshold = configuration.GetValue<int>("VoteThreshold");
            _voteThreshold = configuration.GetSection("PetitionSettings")?.GetValue<int>("VoteThreshold") ?? 0;
            _logger.LogInformation("PetitionNotificationService started");


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckPetitionThreshold();
                
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private async Task CheckPetitionThreshold()
        {
            var petitions = await _petitionRepository.GetAllPetitionsAsync();

            foreach (var petition in petitions)
            {
                if (petition.Votes.Count >= _voteThreshold)
                {
                    // Send notification to all users with Moderator or Staff-admin role on Petition department
                    var userIds = await _userRepository.GetUserIdsByRoleAsync(new List<string> {"Moderator", "Staff-admin"});
                    foreach (var userId in userIds)
                    {
                        await _emailService.SendNotificationAsync(userId.ToString(), $"Petition {petition.Id} has reached {_voteThreshold} votes.");
                    }
                }
            }
        }
    }
}