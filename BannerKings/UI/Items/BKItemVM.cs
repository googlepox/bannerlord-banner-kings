using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace BannerKings.UI.Items
{
    public class BKItemVM : SelectorItemVM
    {
        public BKItemVM(Enum policy, bool isAvailable, string hint, TextObject name = null) : base("")
        {
            Value = (int) (object) policy;
            StringItem = name != null ? name.ToString() : policy.ToString().Replace("_", " ");
            CanBeSelected = isAvailable;
            Hint = new HintViewModel(new TextObject("{=!}" + hint));
        }

        public BKItemVM(int index, bool isAvailable, TextObject hint, TextObject name) : base("")
        {
            Value = index;
            StringItem = name.ToString();
            CanBeSelected = isAvailable;
            Hint = new HintViewModel(hint);
        }

        public int Value { get; }
    }
}