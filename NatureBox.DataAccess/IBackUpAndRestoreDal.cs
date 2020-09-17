﻿using System.Threading.Tasks;

namespace NatureBox.DataAccess
{
    public interface IBackUpAndRestoreRepository
    {
        Task<bool> BackUp(string folderPath);
        Task<bool> Restore(string filePath);
    }
}