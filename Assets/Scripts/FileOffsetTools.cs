using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileOffsetTools  
{
    private static byte[] buffer = new byte[50];

    private static byte[] mark = new byte[8] { 0, 111, 80, 40, 155, 142, 133, 200 };

    public static bool setAttribute = true;




    public static void Encode(string path)
    {
        if (!File.Exists(path)) return;

        if (setAttribute)
        {
            FileAttributes attr = File.GetAttributes(path);
            //attr = attr & (~FileAttributes.Hidden) & (~FileAttributes.System);
            attr = FileAttributes.Hidden;
            File.SetAttributes(path, attr);
        }


        FileStream fs = new FileStream(path, FileMode.Open);
        Encode(fs);
        fs.Flush();
        fs.Close();
    }


    public static void Decode(string path)
    {
        if (!File.Exists(path)) return;

        if (setAttribute)
        {
            FileAttributes attr = File.GetAttributes(path);
            attr = attr | FileAttributes.Hidden | FileAttributes.System;

            File.SetAttributes(path, attr);
        }

        FileStream fs = new FileStream(path, FileMode.Open);
        Decode(fs);
        fs.Flush();
        fs.Close();
    }


    private static void Encode(Stream s)
    {
        if (CheckMark(s)) return;

        s.Position = 0;
        int count = s.Read(buffer, 0, buffer.Length);

        if (count < buffer.Length) return;

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)~buffer[i];
        }

        s.Position = 0;

        s.Write(buffer, 0, count);

        AddMark(s);
    }




    private static void Decode( Stream s)
    {
        if (!CheckMark(s)) return;

        s.Position = 0;
        int count = s.Read(buffer, 0, buffer.Length);

        if (count < buffer.Length) return;


        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)~buffer[i];
        }

        s.Position = 0;

        s.Write(buffer, 0, count);

        RemoveMark(s);
    }




    public static void AddMark(Stream s)
    {
        long pos = s.Length;
        s.SetLength(s.Length+mark.Length);
        s.Position = pos;
        s.Write(mark,0,mark.Length);
        Debug.Log("文件偏移完成！");
    }

    public static void RemoveMark(Stream s)
    {
        s.SetLength(s.Length - mark.Length);
    }
   



    private static bool CheckMark(Stream s)
    {
        if (s.Length < mark.Length) return false;
        //long position = s.Position;


        long pos = s.Length - mark.Length;

        if (pos < 0) return false;


        s.Position = pos;
        s.Read(buffer, 0, mark.Length);

        for (int i = 0; i < mark.Length; i++)
        {
            if (mark[i] != buffer[i]) return false;
        }
        //s.Position = position;
        return true;
    }
}

