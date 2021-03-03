using System;
using System.Collections.Generic;
using System.Text;

namespace CalenderBot
{
    class Response
    {
        public string status;
        public string msg;
        public CalenderData[] data;
        public string code;
    }
    class CalenderData
    {
        public string id;
        public string teachingWeek;
        public string instructorId;
        public string roomId;
        public string classId;
        public string period;
        public string weekDay;
        public string exprPrjId;
        public string serialPeriod;
        public bool notArrangeRoom;
        public bool notArrangeTimeAndRoom;
        public bool wholeWeekOccupy;
        public string hourType;
        public bool virtualFlag;
        public bool overlap;
        public string courseName;
        public string courseCode;
        public string classNbr;
        public string classType;
        public string stuCapacity;
        public string exprProjectName;
        public string exprProjectCode;
        public string roomCapacity;
        public string roomName;
        public string teachingWeekFormat;
        public string periodFormat;
        public string roomBuildingId;
        public string roomBuildingCampusName;
        public string weekDayFormat;
        public string campusId;
        public Instructor[] classTimetableInstrVOList;
    }
    class Instructor
    {
        public string id;
        public string timetableId;
        public string classInstructorId;
        public string instrRoleCategory;
        public string instructorId;
        public string instructorName;
        public string instructorCode;
    }
}
