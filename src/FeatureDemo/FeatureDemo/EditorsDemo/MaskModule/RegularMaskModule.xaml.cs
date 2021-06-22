﻿using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.WinUI.Editors;
using FeatureDemo.Common;

namespace EditorsDemo {
    public sealed partial class RegularMaskModule : DemoSubModuleView {
        public RegularMaskModule() {
            ViewModel = new RegularMaskViewModel();
            this.InitializeComponent();
            Root.Children.Where(x => x is TextEdit).ForEach(x => x.GotFocus += (s, e) => ViewModel.FocusedEditor = (TextEdit)x);
        }

        public RegularMaskViewModel ViewModel { get; }
    }

    public class RegularMaskViewModel : ViewModelBase {
        public TextEdit FocusedEditor {
            get => GetValue<TextEdit>();
            set => SetValue(value, OnFocusedEditorChanged);
        }
        public TextInputMaskSettings TextInputSettings {
            get => GetValue<TextInputMaskSettings>();
            private set => SetValue(value);
        }
        public string Mask {
            get => GetValue<string>();
            set => SetValue(value, OnMaskChanged);
        }

        void OnFocusedEditorChanged() {
            TextInputSettings = FocusedEditor?.TextInputSettings as TextInputMaskSettings;
            Mask = TextInputSettings?.Mask;
        }
        async void OnMaskChanged() {
            if (TextInputSettings == null || Mask == TextInputSettings.Mask)
                return;
            string maskBackup = TextInputSettings.Mask;
            try {
                TextInputSettings.Mask = Mask;
            }
            catch {
                await GetService<IMessageBoxService>().ShowAsync("Invalid mask", "Error");
                TextInputSettings.Mask = maskBackup;
                Mask = maskBackup;
            }
        }
    }
}