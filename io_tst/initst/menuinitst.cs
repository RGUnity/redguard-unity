using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;

public class tst
{
    public static void Main(string[] args)
    {
        RGINIFile inifile = new RGINIFile();
        inifile.LoadFile("../../game_3dfx/MENU.INI");
        Console.WriteLine($"preload_sprites: {inifile.menuData.preload_sprites}");
        Console.WriteLine($"hi_res_menu: {inifile.menuData.hi_res_menu}");
        Console.WriteLine($"ignore_autosave: {inifile.menuData.ignore_autosave}");
        Console.WriteLine($"force_movies: {inifile.menuData.force_movies}");
        foreach(var page in inifile.menuData.pages)
        {
            foreach(var texture_set in page.Value.texture_set)
            {
                Console.WriteLine($"pages_{page.Key}.texture_set{texture_set.Key}: {inifile.menuData.pages[page.Key].texture_set[texture_set.Key]}");
            }
            foreach(var texture_index in page.Value.texture_index)
            {
                Console.WriteLine($"pages_{page.Key}.texture_index{texture_index.Key}: {inifile.menuData.pages[page.Key].texture_index[texture_index.Key]}");
            }
            foreach(var menuItem in page.Value.menuItems)
            {
                Console.WriteLine($"pages_{page.Key}.menuItem[{menuItem.Key}].text_x: {menuItem.Value.text_x}");
            }

            // do something with entry.Value or entry.Key
        }
        //test_map_order;
    }
}
