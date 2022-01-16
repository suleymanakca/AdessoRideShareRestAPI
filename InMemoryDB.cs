using AdessoRideShareRestAPI.Models;

namespace AdessoRideShareRestAPI
{
    public static class InMemoryDB
    {
        public static List<Trip> trips = new List<Trip>();
        public static List<User> users = new List<User>();
        public static List<UserJoin> user_join = new List<UserJoin>();

        public static int AddTrip(Trip trip)
        {
            trip.Id = trips.Count + 1;
            trips.Add(trip);
            return trip.Id;
        }

        public static int AddUser()
        {
            var userCount = users.Count;
            var user = new User { UserId = ++userCount };
            users.Add(user);
            return user.UserId;
        }

        public static int AddUserJoin(UserJoin userjoin)
        {
            var userjoin_Count = user_join.Count;
            userjoin.Id = ++userjoin_Count;
            user_join.Add(userjoin);
            return userjoin.Id;
        }



    }

}
