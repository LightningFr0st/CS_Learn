using menu;
using Microsoft.VisualBasic;
using oop3.DisplayObjects;
using oop3.Menu;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace oop3.Utilities
{
    internal class TemplateGenerator
    {
        public enum Primitive
        {
            P_Rectangle,
            P_Square,
            P_Ellipse,
            P_Circle,
            P_Triangle
        }

        public enum SubMenuDirection
        {
            Dir_Default,
            Dir_Horizontal,
            Dir_Vertical,
            Dir_Ladder
        }

        public Primitive activePrimitive;
        public int templateSizeX;
        public int templateSizeY;
        public Color viewFill;
        public Color viewStroke;
        public int viewStrokeThickness;

        public static int reqX, reqY, reqXt, reqYt;

        public SubMenuDirection direction;
        public static SubMenuDirection MainMenuDirection;
        public int offsetX;
        public int req_offsetX;
        public int offsetY;
        public int req_offsetY;

        public FontFamily fontFamily;
        public Color fontColor;
        public int fontSize;
        public string textData;


        public TemplateGenerator(
            Primitive? primitive = null, int? sizeX = null, int? sizeY = null,
            Color? vFill = null, Color? vStroke = null, int? vStrokeTh = null,
            Color? fColor = null, int? fSize = null
            )
        {
            activePrimitive = primitive == null ? Primitive.P_Rectangle : primitive.Value;
            templateSizeX = sizeX == null ? 100 : sizeX.Value;
            templateSizeY = sizeY == null ? 100 : sizeY.Value;
            viewFill = vFill == null ? Color.Red : vFill.Value;
            viewStroke = vStroke == null ? Color.Black : vStroke.Value;
            viewStrokeThickness = vStrokeTh == null ? 1 : vStrokeTh.Value;

            direction = SubMenuDirection.Dir_Horizontal;
            offsetX = 0;
            req_offsetX = 0;
            offsetY = 0;
            req_offsetY = 0;

            fontFamily = FontFamily.GenericMonospace;
            fontColor = fColor == null ? Color.Black : fColor.Value;
            fontSize = fSize == null ? 14 : fSize.Value;
            textData = "";
        }
        public static string GetPrimitiveString(Primitive primitive)
        {
            string result = "Unknown";
            switch (primitive)
            {
                case Primitive.P_Rectangle:
                    result = "Rectangle";
                    break;
                case Primitive.P_Square:
                    result = "Square";
                    break;
                case Primitive.P_Ellipse:
                    result = "Ellipse";
                    break;
                case Primitive.P_Circle:
                    result = "Circle";
                    break;
                case Primitive.P_Triangle:
                    result = "Triangle";
                    break;
            }
            return result;
        }


        public static TemplateGenerator TemplateFromMenuItem(MenuItem item)
        {
            TemplateGenerator tg = new TemplateGenerator();
            DisplayObject display = item.view;
            TextObject text = item.text;
            tg.templateSizeX = display.frameX2 - display.frameX1;
            tg.templateSizeY = display.frameY2 - display.frameY1;
            tg.activePrimitive = GetPrimitive(display);
            tg.viewFill = display.fillColor;
            tg.viewStroke = display.strokeColor;
            tg.viewStrokeThickness = display.strokeThickness;
            tg.fontColor = text.fillColor;
            tg.fontSize = text.fontSize;
            tg.textData = text.textData;
            return tg;
        }
        private static Primitive GetPrimitive(DisplayObject display)
        {
            Primitive primitive = Primitive.P_Rectangle;
            if (display is RectangleObject)
                primitive = Primitive.P_Rectangle;
            else if (display is EllipseObject)
                primitive = Primitive.P_Ellipse;
            else if (display is TriangleObject)
                primitive = Primitive.P_Triangle;
            else if (display is CircleObject)
                primitive = Primitive.P_Circle;
            else if (display is SquareObject)
                primitive = Primitive.P_Square;
            return primitive;
        }

        private static void ItemEditFromTemplate(Menu.Menu itemEditor, TemplateGenerator template)
        {
            itemEditor.items[0].text.textData = $"Shape\n{GetPrimitiveString(template.activePrimitive)}";
            itemEditor.items[1].text.textData = $"Size\nW:{template.templateSizeX}\nH:{template.templateSizeY}";
            itemEditor.items[2].text.textData = $"Fill color\n";
            itemEditor.items[3].text.textData = $"Border color\n";
            itemEditor.items[4].text.textData = $"Stroke thickness\n{template.viewStrokeThickness}";
            itemEditor.items[5].text.textData = $"Font color\n";
            itemEditor.items[6].text.textData = $"Font size\n{template.fontSize}";
            itemEditor.items[7].text.textData = $"Text \n{template.textData}";
            itemEditor.items[8].text.textData = $"Position\ndata";
        }

        public static Menu.Menu GenItemEdit(TemplateGenerator itemTemplate, MenuItem item)
        {
            TemplateGenerator menuTemplate = new TemplateGenerator(Primitive.P_Rectangle, 140, 80, Color.Azure, null, null, null, 14);
            menuTemplate.offsetX = 20;
            menuTemplate.offsetY = 10;


            MenuItem PrimitiveType = menuTemplate.GetTemplatePrimitive();
            PrimitiveType.offsetX = 0;
            PrimitiveType.offsetY = -110;

            menuTemplate.activePrimitive = Primitive.P_Rectangle;
            MenuItem PrimitiveItem1 = menuTemplate.GetTemplatePrimitive("");
            PrimitiveItem1.clickHandler = () =>
            {
                itemTemplate.activePrimitive = Primitive.P_Rectangle;
                template.activePrimitive = Primitive.P_Rectangle;
                DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                item.view = v;
                item.frameX1 = v.frameX1;
                item.frameX2 = v.frameX2;
                item.frameY1 = v.frameY1;
                item.frameY2 = v.frameY2;
                PrimitiveType.text.textData = $"Shape\n{GetPrimitiveString(itemTemplate.activePrimitive)}";
            };
            PrimitiveType.AddItem(PrimitiveItem1);


            menuTemplate.activePrimitive = Primitive.P_Ellipse;
            MenuItem PrimitiveItem3 = menuTemplate.GetTemplatePrimitive("");
            PrimitiveItem3.clickHandler = () =>
            {
                itemTemplate.activePrimitive = Primitive.P_Ellipse;
                template.activePrimitive = Primitive.P_Ellipse;
                DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                item.view = v;
                item.frameX1 = v.frameX1;
                item.frameX2 = v.frameX2;
                item.frameY1 = v.frameY1;
                item.frameY2 = v.frameY2;
                PrimitiveType.text.textData = $"Shape\n{GetPrimitiveString(itemTemplate.activePrimitive)}";
            };
            PrimitiveType.AddItem(PrimitiveItem3);

            menuTemplate.activePrimitive = Primitive.P_Triangle;
            MenuItem PrimitiveItem5 = menuTemplate.GetTemplatePrimitive("");
            PrimitiveItem5.clickHandler = () =>
            {
                itemTemplate.activePrimitive = Primitive.P_Triangle;
                template.activePrimitive = Primitive.P_Triangle;
                DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                item.view = v;
                item.frameX1 = v.frameX1;
                item.frameX2 = v.frameX2;
                item.frameY1 = v.frameY1;
                item.frameY2 = v.frameY2;
                PrimitiveType.text.textData = $"Shape\n{GetPrimitiveString(itemTemplate.activePrimitive)}";
            };
            PrimitiveType.AddItem(PrimitiveItem5);

            menuTemplate.activePrimitive = Primitive.P_Rectangle;

            MenuItem PrimitiveSize = menuTemplate.GetTemplatePrimitive();
            PrimitiveSize.clickHandler = () =>
            {
                int[]? data;
                if (GetFormInts(["Width", "Height"], [itemTemplate.templateSizeX, itemTemplate.templateSizeY], 2, out data))
                {
                    if (data != null)
                    {
                        itemTemplate.templateSizeX = data[0];
                        itemTemplate.templateSizeY = data[1];
                        template.templateSizeX = data[0];
                        template.templateSizeY = data[1];
                        DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                        v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                        item.view = v;
                        item.frameX1 = v.frameX1;
                        item.frameX2 = v.frameX2;
                        item.frameY1 = v.frameY1;
                        item.frameY2 = v.frameY2;
                        TextObject t = itemTemplate.GetTemplatePrimitive().text;
                        t.ShiftObject(item.view.frameX1 - t.frameX1, item.view.frameY1 - t.frameY1);
                        item.text = t;

                        PrimitiveSize.text.textData = $"Size\nW:{data[0]}\nH:{data[1]}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem PrimitiveFill = menuTemplate.GetTemplatePrimitive();
            PrimitiveFill.view.fillColor = menuTemplate.viewFill;
            PrimitiveFill.clickHandler = () =>
            {
                int[]? data;
                if (GetColor(3, out data))
                {
                    if (data != null)
                    {
                        itemTemplate.viewFill = Color.FromArgb(data[0], data[1], data[2]);
                        template.viewFill = Color.FromArgb(data[0], data[1], data[2]);
                        PrimitiveFill.setViewColor(Color.FromArgb(data[0], data[1], data[2]));

                        //DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                        //v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                        //item.view = v;
                        item.view.fillColor = itemTemplate.viewFill;
                        PrimitiveFill.text.textData = $"Fill:";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem PrimitiveStroke = menuTemplate.GetTemplatePrimitive();
            PrimitiveStroke.fillColor = menuTemplate.viewStroke;
            PrimitiveStroke.clickHandler = () =>
            {
                int[]? data;
                if (GetColor(3, out data))
                {
                    if (data != null)
                    {
                        itemTemplate.viewStroke = Color.FromArgb(data[0], data[1], data[2]);
                        template.viewStroke = Color.FromArgb(data[0], data[1], data[2]);
                        PrimitiveStroke.setViewColor(Color.FromArgb(data[0], data[1], data[2]));
                        //DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                        //v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                        //item.view = v;
                        item.view.strokeColor = itemTemplate.viewStroke;
                        PrimitiveStroke.text.textData = $"Border:\n";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem PrimitiveStrokeThick = menuTemplate.GetTemplatePrimitive();
            PrimitiveStrokeThick.clickHandler = () =>
            {
                int[]? data;
                if (GetInts("Enter stroke thickness", 1, out data))
                {
                    if (data != null)
                    {
                        itemTemplate.viewStrokeThickness = data[0];
                        template.viewStrokeThickness = data[0];
                        //DisplayObject v = itemTemplate.GetTemplatePrimitive().view;
                        //v.ShiftObject(item.view.frameX1 - v.frameX1, item.view.frameY1 - v.frameY1);
                        //item.view = v;
                        item.view.strokeThickness = itemTemplate.viewStrokeThickness;
                        PrimitiveStrokeThick.text.textData = $"Border thick\n{data[0]}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem FontColor = menuTemplate.GetTemplatePrimitive();
            FontColor.view.fillColor = menuTemplate.viewFill;
            FontColor.clickHandler = () =>
            {
                int[]? data;
                if (GetColor(3, out data))
                {
                    if (data != null)
                    {
                        itemTemplate.fontColor = Color.FromArgb(data[0], data[1], data[2]);
                        template.fontColor = Color.FromArgb(data[0], data[1], data[2]);
                        FontColor.setViewColor(Color.FromArgb(data[0], data[1], data[2]));
                        TextObject t = itemTemplate.GetTemplatePrimitive().text;
                        t.ShiftObject(item.view.frameX1 - t.frameX1, item.view.frameY1 - t.frameY1);
                        item.text = t;
                        FontColor.text.textData = $"Font:\n";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem FontSize = menuTemplate.GetTemplatePrimitive();
            FontSize.clickHandler = () =>
            {
                int[]? data;
                if (GetInts("Enter font size", 1, out data))
                {
                    if (data != null)
                    {
                        itemTemplate.fontSize = data[0];
                        template.fontSize = data[0];
                        TextObject t = itemTemplate.GetTemplatePrimitive().text;
                        t.ShiftObject(item.view.frameX1 - t.frameX1, item.view.frameY1 - t.frameY1);
                        item.text = t;
                        FontSize.text.textData = $"Font size\n{data[0]}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem TextData = menuTemplate.GetTemplatePrimitive();
            TextData.clickHandler = () =>
            {
                itemTemplate.textData = Interaction.InputBox("Enter text data", "", itemTemplate.textData);
                template.textData = itemTemplate.textData;
                TextObject t = itemTemplate.GetTemplatePrimitive().text;
                t.ShiftObject(item.view.frameX1 - t.frameX1, item.view.frameY1 - t.frameY1);
                item.text = t;
                TextData.text.textData = $"Text \n{itemTemplate.textData}";
            };

            MenuItem PosData = menuTemplate.GetTemplatePrimitive();
            PosData.clickHandler = () =>
            {
                int[]? data;
                if (GetFormInts(["X", "Y"], [item.view.frameX1, item.view.frameY1], 2, out data))
                {
                    if (data != null)
                    {
                        item.ShiftObject(data[0] - item.view.frameX1, data[1] - item.view.frameY1);
                    }
                }
            };

            Menu.Menu menu = new Menu.Menu(5, 800);

            int offsx, offsy;
            menuTemplate.direction = SubMenuDirection.Dir_Horizontal;
            (offsx, offsy) = menuTemplate.GenerateOffsets(10, 10, SubMenuDirection.Dir_Horizontal);
            menu.offsetX = offsx;
            menu.offsetY = offsy;

            menu.AddItem(PrimitiveType);
            menu.AddItem(PrimitiveSize);
            menu.AddItem(PrimitiveFill);
            menu.AddItem(PrimitiveStroke);
            menu.AddItem(PrimitiveStrokeThick);
            menu.AddItem(FontColor);
            menu.AddItem(FontSize);
            menu.AddItem(TextData);
            menu.AddItem(PosData);

            menuTemplate.direction = SubMenuDirection.Dir_Vertical;
            menuTemplate.offsetX = 0;
            menuTemplate.offsetY = 100;

            ItemEditFromTemplate(menu, itemTemplate);
            return menu;
        }

        public static TemplateGenerator template;
        public static (Menu.Menu, TemplateGenerator template) GenMenuEdit(Menu.Menu editedMenu)
        {

            template = new TemplateGenerator(Primitive.P_Rectangle, 160, 70, Color.Azure, null, null, null, 10);
            template.offsetX = 20;
            template.offsetY = 10;

            template.textData = "Test";

            MenuItem AddToMenu = template.GetTemplatePrimitive("Add item");
            AddToMenu.clickHandler = () =>
            {
                if (editedMenu.lastClickedItem != null)
                {
                    MenuItem item = editedMenu.lastClickedItem;
                    while (item.lastClickedItem != null)
                    {
                        item = item.lastClickedItem;
                    }
                    item.offsetX = template.offsetX;
                    item.offsetY = template.offsetY;
                    item.AddItem(template.GetTemplatePrimitive());

                }
                else
                {
                    editedMenu.AddItem(template.GetTemplatePrimitive());
                }

                (editedMenu.offsetX, editedMenu.offsetY) = template.GenerateOffsets(editedMenu.req_offsetX, editedMenu.req_offsetY, MainMenuDirection);
                (template.offsetX, template.offsetY) = template.GenerateOffsets(template.req_offsetX, template.req_offsetY, template.direction);

                //int offsx, offsy;

                //template.direction = SubMenuDirection.Dir_Horizontal;
                //(offsx, offsy) = template.GenerateOffsets(30, 0, SubMenuDirection.Dir_Horizontal);
                //editedMenu.offsetX = offsx;
                //editedMenu.offsetY = offsy;

                //template.direction = SubMenuDirection.Dir_Vertical;
                //template.offsetX = 0;
                //template.offsetY = 100;
            };

            MenuItem RemoveFromMenu = template.GetTemplatePrimitive("Delete item");
            RemoveFromMenu.clickHandler = () =>
            {
                if (editedMenu.lastClickedItem != null)
                {
                    MenuItem item = editedMenu.lastClickedItem;
                    if (item.lastClickedItem != null)
                    {
                        while (item.lastClickedItem.lastClickedItem != null)
                        {
                            item = item.lastClickedItem;
                        }
                        MenuItem temp = item.lastClickedItem;
                        item.lastClickedItem = null;

                        item.DeleteItem(temp);
                    }
                    else
                    {
                        editedMenu.lastClickedItem = null;
                        editedMenu.DeleteItem(item);
                    }
                }
                else
                {
                    MessageBox.Show("No item selected");
                }
            };


            MenuItem PrimitiveType = template.GetTemplatePrimitive($"Shape\n{GetPrimitiveString(template.activePrimitive)}");
            PrimitiveType.offsetX = -200;
            PrimitiveType.offsetY = 0;

            template.activePrimitive = Primitive.P_Rectangle;
            MenuItem PrimitiveItem1 = template.GetTemplatePrimitive("");
            PrimitiveItem1.clickHandler = () =>
            {
                template.activePrimitive = Primitive.P_Rectangle;
                PrimitiveType.text.textData = $"Shape\n{GetPrimitiveString(template.activePrimitive)}";
            };
            PrimitiveType.AddItem(PrimitiveItem1);


            template.activePrimitive = Primitive.P_Ellipse;
            MenuItem PrimitiveItem3 = template.GetTemplatePrimitive("");
            PrimitiveItem3.clickHandler = () =>
            {
                template.activePrimitive = Primitive.P_Ellipse;
                PrimitiveType.text.textData = $"Shape\n{GetPrimitiveString(template.activePrimitive)}";
            };
            PrimitiveType.AddItem(PrimitiveItem3);

            template.activePrimitive = Primitive.P_Triangle;
            MenuItem PrimitiveItem5 = template.GetTemplatePrimitive("");
            PrimitiveItem5.clickHandler = () =>
            {
                template.activePrimitive = Primitive.P_Triangle;
                PrimitiveType.text.textData = $"Shape\n{GetPrimitiveString(template.activePrimitive)}";
            };
            PrimitiveType.AddItem(PrimitiveItem5);

            template.activePrimitive = Primitive.P_Rectangle;

            MenuItem PrimitiveSize = template.GetTemplatePrimitive($"Size\nW:{template.templateSizeX}\nH:{template.templateSizeY}");
            PrimitiveSize.clickHandler = () =>
            {
                int[]? data;
                if (GetFormInts(["Width", "Height"], [template.templateSizeX, template.templateSizeY], 2, out data))
                {
                    if (data != null)
                    {
                        template.templateSizeX = data[0];
                        template.templateSizeY = data[1];
                        PrimitiveSize.text.textData = $"Size\nW:{data[0]}\nH:{data[1]}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem PrimitiveFill = template.GetTemplatePrimitive($"Fill color\n");
            PrimitiveFill.view.fillColor = template.viewFill;
            PrimitiveFill.clickHandler = () =>
            {
                int[]? data;
                if (GetColor(3, out data))
                {
                    if (data != null)
                    {
                        template.viewFill = Color.FromArgb(data[0], data[1], data[2]);
                        PrimitiveFill.setViewColor(Color.FromArgb(data[0], data[1], data[2]));
                        PrimitiveFill.text.textData = $"Fill color\n"; ;
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem PrimitiveStroke = template.GetTemplatePrimitive($"Border color\n");
            PrimitiveStroke.fillColor = template.viewStroke;
            PrimitiveStroke.clickHandler = () =>
            {
                int[]? data;
                if (GetColor(3, out data))
                {
                    if (data != null)
                    {
                        template.viewStroke = Color.FromArgb(data[0], data[1], data[2]);
                        PrimitiveStroke.setViewColor(Color.FromArgb(data[0], data[1], data[2]));
                        PrimitiveStroke.text.textData = $"Border color\n";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem PrimitiveStrokeThick = template.GetTemplatePrimitive($"Border Thickness\n{template.viewStrokeThickness}");
            PrimitiveStrokeThick.clickHandler = () =>
            {
                int[]? data;
                if (GetInts("Enter stroke thickness", 1, out data))
                {
                    if (data != null)
                    {
                        template.viewStrokeThickness = data[0];
                        PrimitiveStrokeThick.text.textData = $"Stroke thickness\n{data[0]}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem FontColor = template.GetTemplatePrimitive($"Font color\n");
            FontColor.view.fillColor = template.viewFill;
            FontColor.clickHandler = () =>
            {
                int[]? data;
                if (GetColor(3, out data))
                {
                    if (data != null)
                    {
                        template.fontColor = Color.FromArgb(data[0], data[1], data[2]);
                        FontColor.setViewColor(Color.FromArgb(data[0], data[1], data[2]));
                        FontColor.text.textData = $"Font color\n";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem FontSize = template.GetTemplatePrimitive($"Font size\n{template.fontSize}");
            FontSize.clickHandler = () =>
            {
                int[]? data;
                if (GetInts("Enter font size", 1, out data))
                {
                    if (data != null)
                    {
                        template.fontSize = data[0];
                        FontSize.text.textData = $"Font size\n{data[0]}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid data");
                    }
                }
            };
            MenuItem TextData = template.GetTemplatePrimitive($"Text \n{template.textData}");
            TextData.clickHandler = () =>
            {
                template.textData = Interaction.InputBox("Enter text data", "", template.textData);
                TextData.text.textData = $"Text \n{template.textData}";
            };

            MenuItem OffsetData = template.GetTemplatePrimitive($"Offset");
            OffsetData.clickHandler = () =>
            {
                int[]? data;
                int offsx, offsy;
                if (editedMenu.lastClickedItem != null)
                {
                    if (GetFormInts(["dX", "dY"], [Math.Max((template.offsetX - template.templateSizeX), 0), Math.Max((template.offsetY - template.templateSizeY), 0)], 2, out data))
                    {
                        if (data != null)
                        {
                            if (data[0] != 0 && data[1] != 0)
                            {
                                template.direction = SubMenuDirection.Dir_Ladder;
                            }
                            else if (data[0] != 0 && data[1] == 0)
                            {
                                template.direction = SubMenuDirection.Dir_Horizontal;
                            }
                            else
                            {
                                template.direction = SubMenuDirection.Dir_Vertical;
                            }
                            (offsx, offsy) = template.GenerateOffsets(data[0], data[1], template.direction);
                            template.offsetX = offsx;
                            template.req_offsetX = data[0];
                            template.offsetY = offsy;
                            template.req_offsetY = data[1];
                        }
                        else
                        {
                            MessageBox.Show("Invalid data");
                        }
                    }
                }
                else
                {
                    if (GetFormInts(["dX", "dY"], [Math.Max((editedMenu.offsetX - template.templateSizeX), 0), Math.Max((editedMenu.offsetY - template.templateSizeY), 0)], 2, out data))
                    {
                        if (data != null)
                        {
                            if (data[0] != 0 && data[1] != 0)
                            {
                                (offsx, offsy) = template.GenerateOffsets(data[0], data[1], SubMenuDirection.Dir_Ladder);
                                MainMenuDirection = SubMenuDirection.Dir_Ladder;
                            }
                            else if (data[0] != 0 && data[1] == 0)
                            {
                                (offsx, offsy) = template.GenerateOffsets(data[0], data[1], SubMenuDirection.Dir_Horizontal);
                                MainMenuDirection = SubMenuDirection.Dir_Horizontal;
                            }
                            else
                            {
                                (offsx, offsy) = template.GenerateOffsets(data[0], data[1], SubMenuDirection.Dir_Vertical);
                                MainMenuDirection = SubMenuDirection.Dir_Vertical;
                            }

                            editedMenu.offsetX = offsx;
                            editedMenu.req_offsetX = data[0];
                            editedMenu.offsetY = offsy;
                            editedMenu.req_offsetY = data[1];
                        }
                        else
                        {
                            MessageBox.Show("Invalid data");
                        }
                    }

                }
            };

            Menu.Menu menu = new Menu.Menu(1260, 0);

            int offsx, offsy;
            template.direction = SubMenuDirection.Dir_Vertical;
            (offsx, offsy) = template.GenerateOffsets(0, 10, SubMenuDirection.Dir_Vertical);
            menu.offsetX = offsx;
            menu.offsetY = offsy;

            menu.AddItem(AddToMenu);
            menu.AddItem(RemoveFromMenu);
            menu.AddItem(PrimitiveSize);
            menu.AddItem(PrimitiveFill);
            menu.AddItem(PrimitiveStroke);
            menu.AddItem(PrimitiveStrokeThick);
            menu.AddItem(FontColor);
            menu.AddItem(FontSize);
            menu.AddItem(TextData);
            menu.AddItem(PrimitiveType);
            menu.AddItem(OffsetData);

            template.direction = SubMenuDirection.Dir_Horizontal;
            MainMenuDirection = SubMenuDirection.Dir_Horizontal;
            editedMenu.req_offsetX = 30;
            editedMenu.req_offsetY = 0;
            (offsx, offsy) = template.GenerateOffsets(30, 0, SubMenuDirection.Dir_Horizontal);
            editedMenu.offsetX = offsx;
            editedMenu.offsetY = offsy;

            template.direction = SubMenuDirection.Dir_Vertical;
            template.offsetX = 0;
            template.req_offsetX = 0;
            template.offsetY = 20;
            template.req_offsetY = 20;

            return (menu, template);
        }

        public static bool GetFormInts(string[] title, int[] values, int intCount, out int[]? result)
        {
            result = new int[intCount];
            InputForm inputForm = new InputForm(title, values);
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                result[0] = inputForm.InputValue1;
                result[1] = inputForm.InputValue2;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public static bool GetInts(string title, int intCount, out int[]? result)
        {
            result = new int[intCount];
            string res = Interaction.InputBox(title);
            if (res == "")
            {
                return false;
            }
            string[] arr = res.Split(" ");
            if (arr.Length != intCount)
            {
                result = null;
                return true;
            }
            else
            {
                bool valid;
                valid = true;
                int i = 0;
                while (valid && i < intCount)
                {
                    valid = int.TryParse(arr[i], out result[i]);
                    i++;
                }
                if (!valid)
                {
                    result = null;
                }
                return true;
            }
        }

        public static bool GetColor(int intCount, out int[]? result)
        {
            result = new int[intCount];
            ColorDialog colorDialog = new ColorDialog();
            DialogResult resultColor = colorDialog.ShowDialog();
            if (resultColor == DialogResult.OK)
            {
                result[0] = colorDialog.Color.R;
                result[1] = colorDialog.Color.G;
                result[2] = colorDialog.Color.B;
            }
            return true;
        }

        public (int, int) GenerateOffsets(int reqX, int reqY, SubMenuDirection dir_p)
        {
            int offsX = 0, offsY = 0;
            switch (dir_p)
            {
                case (SubMenuDirection.Dir_Vertical):
                    offsX = 0;
                    offsY = templateSizeY + reqY;
                    break;

                case (SubMenuDirection.Dir_Horizontal):
                    offsX = templateSizeX + reqX;
                    offsY = 0;
                    break;

                case (SubMenuDirection.Dir_Ladder):
                    offsX = templateSizeX + reqX;
                    offsY = templateSizeY + reqY;
                    break;

                case (SubMenuDirection.Dir_Default):
                default:
                    offsX = reqX;
                    offsY = reqY;
                    break;
            }
            return (offsX, offsY);
        }

        public MenuItem GetTemplatePrimitive(string? textData = null)
        {
            textData = textData == null ? this.textData : textData;
            int frameX1 = -(templateSizeX / 2);
            int frameY1 = -(templateSizeY / 2);
            int frameX2 = (templateSizeX / 2);
            int frameY2 = (templateSizeY / 2);

            DisplayObject displayObject;
            TextObject text = new TextObject(textData, fontFamily, fontSize, fontColor,
                frameX1, frameY1, frameX2, frameY2
                );
            switch (activePrimitive)
            {
                case Primitive.P_Rectangle:
                    displayObject = new RectangleObject(frameX1, frameY1, frameX2, frameY2, viewFill);
                    break;
                case Primitive.P_Square:
                    displayObject = new SquareObject(frameX1, frameY1, templateSizeX, viewFill);
                    break;
                case Primitive.P_Ellipse:
                    displayObject = new EllipseObject(0, 0, templateSizeX / 2, templateSizeY / 2, viewFill);
                    break;
                case Primitive.P_Circle:
                    displayObject = new CircleObject(0, 0, templateSizeX / 2, viewFill);
                    break;
                case Primitive.P_Triangle:
                    displayObject = new TriangleObject(frameX1, frameY1, templateSizeX, templateSizeY, viewFill);
                    break;
                default:
                    displayObject = new RectangleObject(frameX1, frameY1, frameX2, frameY2, viewFill);
                    break;
            }
            displayObject.SetStrokeThickness(viewStrokeThickness);
            displayObject.strokeColor = viewStroke;

            MenuItem item = new MenuItem(displayObject, text);

            item.offsetX = this.offsetX;
            item.offsetY = this.offsetY;

            //int offsX, offsY;
            //(offsX, offsY) = GenerateOffsets(offsetX, offsetY, direction);
            //item.offsetX = offsX;
            //item.offsetY = offsY;

            return item;
        }
    }
}
