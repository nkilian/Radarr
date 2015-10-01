﻿using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Twitter
{
    public class TwitterSettingsValidator : AbstractValidator<TwitterSettings>
    {
        public TwitterSettingsValidator()
        {
            RuleFor(c => c.AccessToken).NotEmpty();
            RuleFor(c => c.AccessTokenSecret).NotEmpty();
            //TODO: Validate that it is a valid username (numbers, letters and underscores - I think)
            RuleFor(c => c.Mention).NotEmpty().When(c => c.DirectMessage);

            RuleFor(c => c.DirectMessage).Equal(true)
                                         .WithMessage("Using Direct Messaging is recommended, or use a private account.")
                                         .AsWarning();
        }
    }

    public class TwitterSettings : IProviderConfig
    {
        private static readonly TwitterSettingsValidator Validator = new TwitterSettingsValidator();

        public TwitterSettings()
        {
            DirectMessage = true;
            AuthorizeNotification = "step1";
        }

        [FieldDefinition(0, Label = "Access Token", Advanced = true)]
        public string AccessToken { get; set; }

        [FieldDefinition(1, Label = "Access Token Secret", Advanced = true)]
        public string AccessTokenSecret { get; set; }

        [FieldDefinition(2, Label = "Mention", HelpText = "Mention this user in sent tweets")]
        public string Mention { get; set; }

        [FieldDefinition(3, Label = "Direct Message", Type = FieldType.Checkbox, HelpText = "Send a direct message instead of a public message")]
        public bool DirectMessage { get; set; }

        [FieldDefinition(4, Label = "Connect to twitter", Type = FieldType.Action)]
        public string AuthorizeNotification { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}