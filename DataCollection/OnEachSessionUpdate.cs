// This file is part of iRacingPitCrew Application.
//
// Copyright 2014 Dean Netherton
// https://github.com/vipoo/iRacingPitCrew.Net
//
// iRacingPitCrew is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// iRacingPitCrew is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with iRacingPitCrew.  If not, see <http://www.gnu.org/licenses/>.

using iRacingSDK;
using System;

namespace iRacingPitCrew.DataCollection
{
    public static class OnEachSessionUpdate
    {
        public static Func<DataSample, bool> Capture(Func<DataSample, bool> next)
        {
            var lastSessionInfoUpdate = -1;

            return data =>
            {
                if (!data.IsConnected || data.SessionData.InfoUpdate == lastSessionInfoUpdate)
                    return true;

                lastSessionInfoUpdate = data.SessionData.InfoUpdate;
                return next(data);
            };
        }
    }
}
