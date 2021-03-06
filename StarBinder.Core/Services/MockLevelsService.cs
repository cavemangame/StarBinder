﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    [Obsolete("удалить в проекте финфона")]
    public class MockLevelsService : ILevelsService
    {
        private readonly string testLevel = @"
{""Name"":""Test Level"",""Description"":""Test level description"",""Number"":13,""StepsSilver"":8,""StepsGold"":6,""BestSolve"":[0,1,2,3,4,5],""States"":[{""Id"":""1f7131fa-8e68-4f60-be8f-9f78afb7d2a0"",""Color"":""#FF000000""},{""Id"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""Color"":""#FFFFFAFA""}],""Stars"":[{""StateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""FinalStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""InitialStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""RotateAngle"":35.0,""SubBeamsCoeff"":0.5,""InnerCoeff"":0.17,""IsSubBeams"":true,""Beams"":2,""HalfWidthRel"":0.07,""FrontAngle"":2.0,""FrontScale"":0.7,""XRel"":0.725,""YRel"":0.33333333333333331,""Number"":0},{""StateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""FinalStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""InitialStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""RotateAngle"":30.0,""SubBeamsCoeff"":0.5,""InnerCoeff"":0.3,""IsSubBeams"":true,""Beams"":4,""HalfWidthRel"":0.07,""FrontAngle"":2.0,""FrontScale"":0.7,""XRel"":0.2475,""YRel"":0.53,""Number"":1},{""StateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""FinalStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""InitialStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""RotateAngle"":15.0,""SubBeamsCoeff"":0.5,""InnerCoeff"":0.3,""IsSubBeams"":true,""Beams"":5,""HalfWidthRel"":0.07,""FrontAngle"":2.0,""FrontScale"":0.7,""XRel"":0.7525,""YRel"":0.55333333333333334,""Number"":2},{""StateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""FinalStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""InitialStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""RotateAngle"":15.0,""SubBeamsCoeff"":0.5,""InnerCoeff"":0.3,""IsSubBeams"":true,""Beams"":4,""HalfWidthRel"":0.07,""FrontAngle"":2.0,""FrontScale"":0.7,""XRel"":0.275,""YRel"":0.31333333333333335,""Number"":3},{""StateId"":""1f7131fa-8e68-4f60-be8f-9f78afb7d2a0"",""FinalStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""InitialStateId"":""1f7131fa-8e68-4f60-be8f-9f78afb7d2a0"",""RotateAngle"":13.0,""SubBeamsCoeff"":0.5,""InnerCoeff"":0.2,""IsSubBeams"":true,""Beams"":3,""HalfWidthRel"":0.07,""FrontAngle"":-2.0,""FrontScale"":0.7,""XRel"":0.5175,""YRel"":0.16166666666666665,""Number"":4},{""StateId"":""1f7131fa-8e68-4f60-be8f-9f78afb7d2a0"",""FinalStateId"":""f891ce35-2ada-4238-b123-7a3f00f1481e"",""InitialStateId"":""1f7131fa-8e68-4f60-be8f-9f78afb7d2a0"",""RotateAngle"":15.0,""SubBeamsCoeff"":0.5,""InnerCoeff"":0.3,""IsSubBeams"":true,""Beams"":4,""HalfWidthRel"":0.06,""FrontAngle"":2.0,""FrontScale"":0.7,""XRel"":0.46,""YRel"":0.705,""Number"":5}],""Links"":[{""From"":1,""To"":2,""Direction"":0},{""From"":2,""To"":0,""Direction"":0},{""From"":0,""To"":3,""Direction"":0},{""From"":3,""To"":1,""Direction"":0},{""From"":4,""To"":3,""Direction"":0},{""From"":4,""To"":0,""Direction"":0},{""From"":5,""To"":2,""Direction"":0},{""From"":5,""To"":1,""Direction"":0}]}
        ";

        public MockLevelsService()
        {
        }
        
        public Task<Galaxy> GetLevelModel(int level)
        {
            return Task.FromResult(SerializationHelper.GalaxyFromJson(testLevel));
        }

        public Task<bool> IsNextLevel(Galaxy level)
        {
            return Task.FromResult(true);
        }

        public Task UpdateHighScore(int chapter, int level, int steps)
        {
            throw new NotImplementedException();
        }
    }
}
