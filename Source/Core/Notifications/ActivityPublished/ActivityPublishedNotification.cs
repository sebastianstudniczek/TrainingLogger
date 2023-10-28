using MediatR;

namespace TrainingLogger.Core.Notifications.ActivityPublished;

// TODO: ONly create for now which has to be downloaded separately anway
public record ActivityPublishedNotification(ulong ActivityId) : INotification;
