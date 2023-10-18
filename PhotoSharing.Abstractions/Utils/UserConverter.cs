using PhotoSharing.Abstractions.Models.Common;
using PhotoSharing.Core.Models.Auth;

namespace PhotoSharing.Abstractions.Utils
{
    public static class UserConverter
    {

        /// <returns>Returns null if input is null</returns>
        public static UserOutputModel ConvertUserToOutputModel(this User user, bool needToConvertFriends = true)
        {
            if (user == null)
                return null;

            var outputModel = new UserOutputModel()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Name
            };

            if (needToConvertFriends)
            {
                if (user.Subscriptions != null && user.Subscriptions.Any())
                {
                    var subscriptions = user.Subscriptions.Select(x => x.UserSecond.ConvertUserToOutputModel(needToConvertFriends: false)).ToList();
                    outputModel.Subscriptions = subscriptions;
                }
                
                if (user.Subscribers != null && user.Subscribers.Any())
                {
                    var subscribers = user.Subscribers.Select(x => x.UserFirst.ConvertUserToOutputModel(needToConvertFriends: false)).ToList();
                    outputModel.Subscribers = subscribers;
                }
            }

            if (user.Files != null && user.Files.Any())
            {
                foreach (var item in user.Files)
                {
                    outputModel.Files.Add(item.ConvertToServiceOutputModel());
                }
            }

            return outputModel;
        }
    }
}
