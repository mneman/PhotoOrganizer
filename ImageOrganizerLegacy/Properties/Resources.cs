// Decompiled with JetBrains decompiler
// Type: ImageOrganizer.Properties.Resources
// Assembly: ImageOrganizer, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C6C5403-2D15-4A6F-A6E0-305483E9F449
// Assembly location: D:\ImageOrganizer.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ImageOrganizer.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) ImageOrganizer.Properties.Resources.resourceMan, (object) null))
          ImageOrganizer.Properties.Resources.resourceMan = new ResourceManager("ImageOrganizer.Properties.Resources", typeof (ImageOrganizer.Properties.Resources).Assembly);
        return ImageOrganizer.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return ImageOrganizer.Properties.Resources.resourceCulture;
      }
      set
      {
        ImageOrganizer.Properties.Resources.resourceCulture = value;
      }
    }

    internal static string Application_HelpText
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Application_HelpText), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Application_PressAnyKeyToExit
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Application_PressAnyKeyToExit), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Application_Started
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Application_Started), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Application_Title
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Application_Title), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Error_InputDirectoryNotFound
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Error_InputDirectoryNotFound), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Error_InvalidDirectoriesList
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Error_InvalidDirectoriesList), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Error_InvalidInputDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Error_InvalidInputDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Error_InvalidOutputDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Error_InvalidOutputDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_CleaningSourceDirectories
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_CleaningSourceDirectories), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_DirectorySorted
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_DirectorySorted), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_ImageDirectoryMergerInitialized
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_ImageDirectoryMergerInitialized), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_ImageRotated
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_ImageRotated), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_ImageSorterInitialized
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_ImageSorterInitialized), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_LaunchingPreview
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_LaunchingPreview), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_MovingFile
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_MovingFile), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_ProcessingDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_ProcessingDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_ProcessingFile
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_ProcessingFile), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Info_SortingDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Info_SortingDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Operation_Merging
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Operation_Merging), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Operation_Sorting
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Operation_Sorting), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_DirectoriesDeleted
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_DirectoriesDeleted), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_DirectoriesProcessed
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_DirectoriesProcessed), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_Errors
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_Errors), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_FileNameConflictsResolved
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_FileNameConflictsResolved), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_FilesProcessed
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_FilesProcessed), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_ImagesRotated
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_ImagesRotated), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_OperationDuration
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_OperationDuration), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Summary_SubDirectoriesCreated
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Summary_SubDirectoriesCreated), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string TemporaryFileNameFormat
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (TemporaryFileNameFormat), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_Clean
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_Clean), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_False
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_False), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_ImageRotation
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_ImageRotation), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_InputDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_InputDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_ItemValueString
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_ItemValueString), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_MoreInputDirectories
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_MoreInputDirectories), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_None
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_None), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_NotApplicable
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_NotApplicable), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_Operation
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_Operation), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_OutputDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_OutputDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_Preview
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_Preview), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_Progress
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_Progress), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_ProgressBeginning
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_ProgressBeginning), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_ProgressChunk
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_ProgressChunk), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_ProgressEnd
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_ProgressEnd), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_SummaryTitle
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_SummaryTitle), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_True
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_True), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string View_Verbose
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (View_Verbose), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_DeletingNonEmptyDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_DeletingNonEmptyDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_FailedToProcessFile
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_FailedToProcessFile), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_FailedToRetrieveCreationDate
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_FailedToRetrieveCreationDate), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_FailedToRetrieveOrientation
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_FailedToRetrieveOrientation), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_FailedToStartPreviewDueToInvalidFirstImage
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_FailedToStartPreviewDueToInvalidFirstImage), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_FailedToStartPreviewDueToInvalidOutputDirectory
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_FailedToStartPreviewDueToInvalidOutputDirectory), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }

    internal static string Warning_NoCommandLineArgs
    {
      get
      {
        return ImageOrganizer.Properties.Resources.ResourceManager.GetString(nameof (Warning_NoCommandLineArgs), ImageOrganizer.Properties.Resources.resourceCulture);
      }
    }
  }
}
