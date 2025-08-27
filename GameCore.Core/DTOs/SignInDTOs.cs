using System;
using System.Collections.Generic;

namespace GameCore.Core.DTOs
{
    public class SignInResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ConsecutiveDays { get; set; }
        public int TotalDays { get; set; }
        public List<SignInReward> Rewards { get; set; } = new();
        public DateTime SignInDate { get; set; }
    }

    public class SignInStatusResult
    {
        public bool CanSignIn { get; set; }
        public int ConsecutiveDays { get; set; }
        public int TotalDays { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public DateTime? NextSignInDate { get; set; }
        public List<SignInReward> AvailableRewards { get; set; } = new();
    }

    public class SignInRecordDto
    {
        public int SignInId { get; set; }
        public int UserId { get; set; }
        public DateTime SignInDate { get; set; }
        public int ConsecutiveDays { get; set; }
        public List<SignInReward> Rewards { get; set; } = new();
    }

    public class SignInStatisticsDto
    {
        public int TotalSignIns { get; set; }
        public int MaxConsecutiveDays { get; set; }
        public int CurrentConsecutiveDays { get; set; }
        public DateTime FirstSignInDate { get; set; }
        public DateTime LastSignInDate { get; set; }
        public List<SignInCalendarDto> Calendar { get; set; } = new();
    }

    public class SignInCalendarDto
    {
        public DateTime Date { get; set; }
        public bool SignedIn { get; set; }
        public List<SignInReward> Rewards { get; set; } = new();
    }

    public class SignInReward
    {
        public string Type { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
} 