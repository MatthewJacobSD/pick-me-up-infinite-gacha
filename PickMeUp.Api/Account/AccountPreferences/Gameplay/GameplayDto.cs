namespace PickMeUp.Api.Account.AccountPreferences.Gameplay
{
    public sealed class GameplayDto
    {
        public int VisibleActionBars { get; init; }
        public bool ShowCooldownNumbers { get; init; }
        public bool ShowKeybindLabels { get; init; }
        public bool LockActionBars { get; init; }

        public bool ShowDamageNumbers { get; init; }
        public bool ShowHealingNumbers { get; init; }
        public bool ShowCriticalEffects { get; init; }
        public bool ShowFloatingCombatText { get; init; }

        public bool InvertYAxis { get; init; }
        public bool InvertXAxis { get; init; }
        public float CameraSensitivity { get; init; }
        public float FieldOfView { get; init; }

        public bool AutoLoot { get; init; }
        public bool HighlightInteractables { get; init; }

        public bool ShowNameplates { get; init; }
        public bool ShowEnemyNameplates { get; init; }
        public bool ShowFriendlyNameplates { get; init; }
        public bool ShowPingIndicators { get; init; }

        public bool AutoRunToggle { get; init; }
        public bool ToggleSprint { get; init; }

        public int Version { get; init; }

        public GameplaySettings ToSettings() => new()
        {
            VisibleActionBars = VisibleActionBars,
            ShowCooldownNumbers = ShowCooldownNumbers,
            ShowKeybindLabels = ShowKeybindLabels,
            LockActionBars = LockActionBars,

            ShowDamageNumbers = ShowDamageNumbers,
            ShowHealingNumbers = ShowHealingNumbers,
            ShowCriticalEffects = ShowCriticalEffects,
            ShowFloatingCombatText = ShowFloatingCombatText,

            InvertYAxis = InvertYAxis,
            InvertXAxis = InvertXAxis,
            CameraSensitivity = CameraSensitivity,
            FieldOfView = FieldOfView,

            AutoLoot = AutoLoot,
            HighlightInteractables = HighlightInteractables,

            ShowNameplates = ShowNameplates,
            ShowEnemyNameplates = ShowEnemyNameplates,
            ShowFriendlyNameplates = ShowFriendlyNameplates,
            ShowPingIndicators = ShowPingIndicators,

            AutoRunToggle = AutoRunToggle,
            ToggleSprint = ToggleSprint
        };
    }

    public sealed class GameplayPatchDto
    {
        public int? VisibleActionBars { get; init; }
        public bool? ShowCooldownNumbers { get; init; }
        public bool? ShowKeybindLabels { get; init; }
        public bool? LockActionBars { get; init; }

        public bool? ShowDamageNumbers { get; init; }
        public bool? ShowHealingNumbers { get; init; }
        public bool? ShowCriticalEffects { get; init; }
        public bool? ShowFloatingCombatText { get; init; }

        public bool? InvertYAxis { get; init; }
        public bool? InvertXAxis { get; init; }
        public float? CameraSensitivity { get; init; }
        public float? FieldOfView { get; init; }

        public bool? AutoLoot { get; init; }
        public bool? HighlightInteractables { get; init; }

        public bool? ShowNameplates { get; init; }
        public bool? ShowEnemyNameplates { get; init; }
        public bool? ShowFriendlyNameplates { get; init; }
        public bool? ShowPingIndicators { get; init; }

        public bool? AutoRunToggle { get; init; }
        public bool? ToggleSprint { get; init; }

        public int Version { get; init; }

        public GameplaySettings ToApply(GameplaySettings current) => current with 
        {
            VisibleActionBars = VisibleActionBars ?? current.VisibleActionBars,
            ShowCooldownNumbers = ShowCooldownNumbers ?? current.ShowCooldownNumbers,
            ShowKeybindLabels = ShowKeybindLabels ?? current.ShowKeybindLabels,
            LockActionBars = LockActionBars ?? current.LockActionBars,

            ShowDamageNumbers = ShowDamageNumbers ?? current.ShowDamageNumbers,
            ShowHealingNumbers = ShowHealingNumbers ?? current.ShowHealingNumbers,
            ShowCriticalEffects = ShowCriticalEffects ?? current.ShowCriticalEffects,
            ShowFloatingCombatText = ShowFloatingCombatText ?? current.ShowFloatingCombatText,

            InvertYAxis = InvertYAxis ?? current.InvertYAxis,
            InvertXAxis = InvertXAxis ?? current.InvertXAxis,
            CameraSensitivity = CameraSensitivity ?? current.CameraSensitivity,
            FieldOfView = FieldOfView ?? current.FieldOfView,

            AutoLoot = AutoLoot ?? current.AutoLoot,
            HighlightInteractables = HighlightInteractables ?? current.HighlightInteractables,

            ShowNameplates = ShowNameplates ?? current.ShowNameplates,
            ShowEnemyNameplates = ShowEnemyNameplates ?? current.ShowEnemyNameplates,
            ShowFriendlyNameplates = ShowFriendlyNameplates ?? current.ShowFriendlyNameplates,
            ShowPingIndicators = ShowPingIndicators ?? current.ShowPingIndicators,

            AutoRunToggle = AutoRunToggle ?? current.AutoRunToggle,
            ToggleSprint = ToggleSprint ?? current.ToggleSprint
        };
    }

}
