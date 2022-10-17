using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class FileNameInfo {
    public List<string> fileNames;

    public FileNameInfo(List<string> fileNames) {
        this.fileNames = fileNames;
    }
}
