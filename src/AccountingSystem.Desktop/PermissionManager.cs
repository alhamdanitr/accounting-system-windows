using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingSystem.Desktop
{
    public class PermissionManager
    {
        private HashSet<string> _userPermissions = new HashSet<string>();

        public void SetPermissions(IEnumerable<string> permissions)
        {
            _userPermissions = new HashSet<string>(permissions);
        }

        public bool HasPermission(string permissionCode)
        {
            if (_userPermissions.Contains("admin")) return true;
            return _userPermissions.Contains(permissionCode);
        }

        public bool CanCreateSale => HasPermission("sales.create");
        public bool CanDeleteSale => HasPermission("sales.delete");
        public bool CanViewReports => HasPermission("reports.view");
        public bool CanManageInventory => HasPermission("inventory.manage");
    }
}
