using BlazorTextEditor.ClassLib.Keyboard;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorTextEditor.ClassLib.TextEditor;

public class TextEditorCursor
{
    public (int rowIndex, int columnIndex) IndexCoordinates { get; set; }
    public int PreferredColumnIndex { get; set; }
    public TextCursorKind TextCursorKind { get; set; }
    
    /// <summary>
    /// TODO: handle control modifier
    /// </summary>
    public static void MoveCursor(KeyboardEventArgs keyboardEventArgs, TextEditorCursor textEditorCursor, TextEditorBase textEditorBase)
    {
        var localIndexCoordinates = textEditorCursor.IndexCoordinates;
        var localPreferredColumnIndex = textEditorCursor.PreferredColumnIndex;

        void MutateIndexCoordinatesAndPreferredColumnIndex(int columnIndex)
        {
            localIndexCoordinates.columnIndex = columnIndex;
            localPreferredColumnIndex = columnIndex;
        }
        
        switch (keyboardEventArgs.Key)
        {
            case KeyboardKeyFacts.MovementKeys.ARROW_LEFT:
            {
                if (localIndexCoordinates.columnIndex == 0)
                {
                    if (localIndexCoordinates.rowIndex != 0)
                    {
                        localIndexCoordinates.rowIndex--;

                        var lengthOfRow = textEditorBase.GetLengthOfRow(localIndexCoordinates.rowIndex);
                        
                        MutateIndexCoordinatesAndPreferredColumnIndex(lengthOfRow);
                    }
                }
                else
                {
                    MutateIndexCoordinatesAndPreferredColumnIndex(localIndexCoordinates.columnIndex - 1);
                }
                
                break;
            }
            case KeyboardKeyFacts.MovementKeys.ARROW_DOWN:
            {
                if (localIndexCoordinates.rowIndex < textEditorBase.RowCount - 1)
                {
                    localIndexCoordinates.rowIndex++;
                    
                    var lengthOfRow = textEditorBase.GetLengthOfRow(localIndexCoordinates.rowIndex);

                    localIndexCoordinates.columnIndex = lengthOfRow < localPreferredColumnIndex
                        ? lengthOfRow
                        : localPreferredColumnIndex;
                }
                
                break;
            }
            case KeyboardKeyFacts.MovementKeys.ARROW_UP:
            {
                if (localIndexCoordinates.rowIndex > 0)
                {
                    localIndexCoordinates.rowIndex--;
                    
                    var lengthOfRow = textEditorBase.GetLengthOfRow(localIndexCoordinates.rowIndex);

                    localIndexCoordinates.columnIndex = lengthOfRow < localPreferredColumnIndex
                        ? lengthOfRow
                        : localPreferredColumnIndex;
                }
                
                break;
            }
            case KeyboardKeyFacts.MovementKeys.ARROW_RIGHT:
            {
                var lengthOfRow = textEditorBase.GetLengthOfRow(localIndexCoordinates.rowIndex);
                
                if (localIndexCoordinates.columnIndex == lengthOfRow &&
                    localIndexCoordinates.rowIndex < textEditorBase.RowCount - 1)
                {
                    MutateIndexCoordinatesAndPreferredColumnIndex(0);
                }
                else
                {
                    MutateIndexCoordinatesAndPreferredColumnIndex(localIndexCoordinates.columnIndex + 1);
                }
                
                break;
            }
            case KeyboardKeyFacts.MovementKeys.HOME:
            {
                MutateIndexCoordinatesAndPreferredColumnIndex(0);
                
                break;
            }
            case KeyboardKeyFacts.MovementKeys.END:
            {
                var lengthOfRow = textEditorBase.GetLengthOfRow(localIndexCoordinates.rowIndex);
                
                MutateIndexCoordinatesAndPreferredColumnIndex(lengthOfRow);
                
                break;
            }
        }

        textEditorCursor.IndexCoordinates = localIndexCoordinates;
        textEditorCursor.PreferredColumnIndex = localPreferredColumnIndex;
    }
}