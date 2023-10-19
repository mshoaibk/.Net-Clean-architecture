using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM_Common.EnumClasses
{
    public enum SubscriptionStatus
    {
        Active = 1,
        Inactive = 2,
        Pending = 3,
        Expired = 4,
        Cancelled = 5,
        Suspended = 6,
        Renewed = 7,
        Upgraded = 8,
        Downgraded = 9,
        Trial = 10,
        PendingCancellation = 11,
        GracePeriod = 12,
        PaymentFailed = 13
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Successful = 2,
        Failed = 3,
        Refunded = 4,
        PartiallyRefunded = 5,
        Disputed = 6,
        Authorized = 7,
        Voided = 8,
        Processing = 9
    }
    public enum TicketStatus
    {
        Open = 1,
        InProgress = 2,
        OnHold = 3,
        PendingReview = 4,
        Closed = 5,
        Rejected = 6,
        Duplicate = 7,
        Escalated = 8,
        AwaitingUserFeedback = 9,
        Deferred = 10,
        Reopened = 11,
        InReview = 12
    }
    public enum ChatType
    {
        company =1,
        employee = 2
    }
}
