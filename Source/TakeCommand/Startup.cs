/*
	This file is part of TakeCommand /L Unleashed
		© 2018-2023 LisiasT : http://lisias.net <support@lisias.net>
		© 2016-2018 LinuxGuruGamer
		© 2015 Sean McDougall

	TakeCommand is licensed as follows:
		* GPL 3.0 : https://www.gnu.org/licenses/gpl-3.0.txt

	TakeCommand /L Unleashed is distributed in the hope that it will be
	useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the GNU General Public License 3.0
	along with TakeCommand /L Unleashed. If not, see <https://www.gnu.org/licenses/>.

*/
using UnityEngine;

namespace TakeCommand
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class Startup : MonoBehaviour
    {
        private void Start()
        {
            Log.force("Version {0}", Version.Text);

            try
            {
                KSPe.Util.Installation.Check<Startup>();
            }
            catch (KSPe.Util.InstallmentException e)
            {
                Log.error(e, this);
                KSPe.Common.Dialogs.ShowStopperErrorBox.Show(e);
            }
        }
    }
}
