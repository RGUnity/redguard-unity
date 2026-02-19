using System;
using System.IO;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGINIFile
	{
        public enum INISection
        {
            inisection_empty = 0,
            inisection_world,
            inisection_menu_general,
            inisection_menu_page0,
            inisection_menu_page1,
            inisection_menu_page2,
            inisection_menu_page3,
            inisection_menu_page4,
            inisection_menu_page5,
            inisection_menu_page6,
            inisection_menu_page7,
        }
        // MENU.INI
        public struct INIMenuData
        {
            public int preload_sprites;
            public int hi_res_menu;
            public int ignore_autosave;
            public int force_movies;
            public Dictionary<int, INIMenuPage> pages;
        }
        public struct INIMenuPage
        {
            public Dictionary<int, int> texture_set;
            public Dictionary<int, int> texture_index;
            public Dictionary<int, INIMenuItem> menuItems;

        }
        public class INIMenuItem
        {
            public int text_x;
            public int text_y;
            public string text;
            public int justify;
            public int selectable;
            public int default_selected;
            public int texture;
            public int action;
            public int grayed;
            public int output_x;
            public int output_y;
            public int slider_min;
            public int slider_max;
        }


        public void ParseMenuLine(string line, int page)
        {
            if(line.Length == 0)
                return;

            INIMenuPage tmp;
            if(!menuData.pages.TryGetValue(page, out tmp))
            {

                    tmp = new INIMenuPage();
                    tmp.texture_set = new Dictionary<int, int>();
                    tmp.texture_index = new Dictionary<int, int>();
                    tmp.menuItems = new Dictionary<int, INIMenuItem>();
                    menuData.pages.Add(page, tmp);
            }

            string[] values;
            string[] parts = line.Split('=');
            string member = parts[0].Trim();
            string val = parts[1].Trim();
            if(member.Contains("[") && member.Contains("]"))
            {
                int itemIndex = int.Parse(member.Substring(member.IndexOf('[')+1,
                                                            member.IndexOf(']')-member.IndexOf('[')-1)); 
                string memberName = member.Substring(0, member.IndexOf('['));
                int tmpi;
                INIMenuItem tmpItem;
                switch(memberName)
                {
                    case "texture_set":
                        if(!menuData.pages[page].texture_set.TryGetValue(itemIndex, out tmpi))
                        {
                            menuData.pages[page].texture_set.Add(itemIndex, 0);
                        }
                        menuData.pages[page].texture_set[itemIndex] = int.Parse(val);
                        break;
                    case "texture_index":
                        if(!menuData.pages[page].texture_index.TryGetValue(itemIndex, out tmpi))
                        {
                            menuData.pages[page].texture_index.Add(itemIndex, 0);
                        }
                        menuData.pages[page].texture_index[itemIndex] = int.Parse(val);
                        break;
                    case "text_x":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].text_x = int.Parse(val);
                        break;
                    case "text_y":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].text_y = int.Parse(val);
                        break;
                    case "text":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].text = val;
                        break;
                    case "justify":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].justify = int.Parse(val);
                        break;
                    case "selectable":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].selectable = int.Parse(val);
                        break;
                    case "default_selected":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].default_selected = int.Parse(val);
                        break;
                    case "texture":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].texture = int.Parse(val);
                        break;
                    case "action":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].action = int.Parse(val);
                        break;
                    case "grayed":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].grayed = int.Parse(val);
                        break;
                    case "output_x":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].output_x = int.Parse(val);
                        break;
                    case "output_y":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].output_y = int.Parse(val);
                        break;
                    case "slider_min":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].slider_min = int.Parse(val);
                        break;
                    case "slider_max":
                        if(!menuData.pages[page].menuItems.TryGetValue(itemIndex, out tmpItem))
                        {
                            menuData.pages[page].menuItems.Add(itemIndex, new INIMenuItem());
                        }
                        menuData.pages[page].menuItems[itemIndex].slider_max = int.Parse(val);
                        break;
                    default:
                        Console.WriteLine($"UNKNOWN: {memberName}");
                        break;
                }
                menuData.pages[page] = tmp;
            }
            else
            {
                // menu metadata
                switch(member)
                {
                    case "preload_sprites":
                        menuData.preload_sprites = int.Parse(val);
                        break;
                    case "hi_res_menu":
                        menuData.hi_res_menu = int.Parse(val);
                        break;
                    case "ignore_autosave":
                        menuData.ignore_autosave = int.Parse(val);
                        break;
                    case "force_movies":
                        menuData.force_movies = int.Parse(val);
                        break;
                    default:
                        break;
                }
            }
            Console.WriteLine(line);
        }
        public void ParseLine(INISection section, string line)
        {
            switch(section)
            {
                case INISection.inisection_menu_general:
                    ParseMenuLine(line, 0);
                    break;
                case INISection.inisection_menu_page0:
                    ParseMenuLine(line, 0);
                    break;
                case INISection.inisection_menu_page1:
                    ParseMenuLine(line, 1);
                    break;
                case INISection.inisection_menu_page2:
                    ParseMenuLine(line, 2);
                    break;
                case INISection.inisection_menu_page3:
                    ParseMenuLine(line, 3);
                    break;
                case INISection.inisection_menu_page4:
                    ParseMenuLine(line, 4);
                    break;
                case INISection.inisection_menu_page5:
                    ParseMenuLine(line, 5);
                    break;
                case INISection.inisection_menu_page6:
                    ParseMenuLine(line, 6);
                    break;
                case INISection.inisection_menu_page7:
                    ParseMenuLine(line, 7);
                    break;

                default:
                    Console.WriteLine("KAPUT");
                    break;
            }
        }

        public INIMenuData menuData;

        public void LoadFile(string filename)
        {
            try
            {
                StreamReader streamReader = new StreamReader(filename);
                string line = streamReader.ReadLine();
                INISection currentSection = INISection.inisection_empty;
                while(line != null)
                {
                    
                    string[] parts = line.Split(';');
                    string line_nocomment = parts[0];
                    switch(line_nocomment)
                    {
                        // MENU.INI
                        case "[general]":
                            currentSection = INISection.inisection_menu_general;
                            menuData = new INIMenuData();
                            menuData.pages = new Dictionary<int, INIMenuPage>();
                            break;
                        case "[page0]":
                            currentSection = INISection.inisection_menu_page0;
                            break;
                        case "[page1]":
                            currentSection = INISection.inisection_menu_page1;
                            break;
                        case "[page2]":
                            currentSection = INISection.inisection_menu_page2;
                            break;
                        case "[page3]":
                            currentSection = INISection.inisection_menu_page3;
                            break;
                        case "[page4]":
                            currentSection = INISection.inisection_menu_page4;
                            break;
                        case "[page5]":
                            currentSection = INISection.inisection_menu_page5;
                            break;
                        case "[page6]":
                            currentSection = INISection.inisection_menu_page6;
                            break;
                        case "[page7]":
                            currentSection = INISection.inisection_menu_page7;
                            break;
                        default:
                            ParseLine(currentSection, line_nocomment);
                            break;
                    }
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }
            catch(Exception ex)
            {
                Exception ex2 = ex;
                while(ex2.InnerException != null)
                {
                    Console.WriteLine($"ex: {ex2.Message}");
                    ex2 = ex2.InnerException;
                }
                throw new Exception($"Failed to load INI file {filename} with error:\n{ex.Message}\nStackTrace:\n${ex.StackTrace}");
            } 
        }
	}
}
