using UnityEngine;
using UnityEditor.IMGUI.Controls; // required for "AdvancedDropdown"

namespace RPG_Project.Editor
{
    public class TeleportDropdownMenu : AdvancedDropdown
    {
        public TeleportDropdownMenu(AdvancedDropdownState state) : base(state) // CTOR
        {
            minimumSize = new Vector2(100, 225); // Set the minimum size of the dropdown
        }

        protected override AdvancedDropdownItem BuildRoot() // Called when the dropdown is opened
        {
            var root = new AdvancedDropdownItem("Teleport to..."); // Create the root item
            var location1 = new AdvancedDropdownItem("Cathedral");
            var location2 = new AdvancedDropdownItem("Desert Temple");
            var location3 = new AdvancedDropdownItem("Bridge 1");
            var location4 = new AdvancedDropdownItem("Bridge 2");
            var location5 = new AdvancedDropdownItem("Lake 1");
            var location6 = new AdvancedDropdownItem("Lake 2");
            var location7 = new AdvancedDropdownItem("Dark Forest");
            var location8 = new AdvancedDropdownItem("Mountain Top");

            root.AddChild(location1);
            root.AddChild(location2);
            root.AddChild(location3);
            root.AddChild(location4);
            root.AddChild(location5);
            root.AddChild(location6);
            root.AddChild(location7);
            root.AddChild(location8);

            return root; // Return the root item
        }

        protected override void ItemSelected(AdvancedDropdownItem item) // Called when an item is selected
        {
            Teleport.TeleportToPresetLocation(item.name);
        }
    }
}
