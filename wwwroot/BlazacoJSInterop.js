window.Blazaco = window.Blazaco || {};
window.Blazaco.Editors = [];
window.helper = {
  getScrollTop: function (id) {
    const el = document.getElementById(id);
    try {
      if (el) {
        return el.scrollTop;
      } else {
        return -1;
      }
    } catch (e) {
      return -1;
    }
  },
  isScrolledToBottom: function (id) {
    const el = document.getElementById(id);
    try {
      if (el) {
        return el.scrollTop >= el.scrollHeight - el.offsetHeight;
      } else {
        return false;
      }
    } catch (e) {
      return false;
    }
  },
  scrollIntoView: function (id) {
    const el = document.getElementById(id);
    try {
      if (el) {
        el.scrollIntoView();
        return true;
      }
      return false;
    } catch (e) {
      return false;
    }
  },
  setFocus: function (control) {
    if (control) {
      control.focus();
      return true;
    }
    return false;
  }
}
window.Blazaco.Editor = {

  InitializeEditor: function (model) {
    let thisEditor = monaco.editor.create(document.getElementById(model.id), model.options);
    if (window.Blazaco.Editors.find(e => e.id === model.id)) {
      return false;
    }
    else {
      window.Blazaco.Editors.push({ id: model.id, editor: thisEditor });
    }
    return true;
  },
  GetValue: function (id) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    return myEditor.editor.getValue();
  },

  GetPosition: function (id) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    return myEditor.editor.getValue();
  },


  RegisterAction: function (id, actionId, actionLabel, dotnetObj) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    var editor = myEditor.editor;

    editor.addAction({
      // An unique identifier of the contributed action.
      id: actionId,

      // A label of the action that will be presented to the user.
      label: actionLabel,

      // An optional array of keybindings for the action.
      keybindings: [
        monaco.KeyMod.CtrlCmd | monaco.KeyCode.F10,
        // chord
        monaco.KeyMod.chord(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_K, monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_M)
      ],

      // A precondition for this action.
      precondition: null,

      // A rule to evaluate on top of the precondition in order to dispatch the keybindings.
      keybindingContext: null,

      contextMenuGroupId: 'navigation',

      contextMenuOrder: 1.5,

      // Method that will be executed when the action is triggered.
      // @param editor The editor instance is passed in as a convinience
      run: function (ed) {
        return dotnetObj.invokeMethodAsync("Execute");
      }
    });
  },



  GetSelectedText: function (id) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    return myEditor.editor.getModel().getValueInRange(myEditor.editor.getSelection());

  },



  GetCurrentLineText: function (id) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    var ed = myEditor.editor;
    var line = ed.getPosition().lineNumber;
    return ed.getModel().getLineContent(line);

  },

  Layout: function (id) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    myEditor.editor.layout()

  },

  AddInlineDecoration: function (id, startLine, startChar, endLine, endChar, cssClass) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    const oldDecorations = [];
    if (myEditor.rangeHighlightDecorationId) {
      oldDecorations.push(myEditor.rangeHighlightDecorationId);
      myEditor.rangeHighlightDecorationId = null;
    }

    const newDecorations = [
      { range: new monaco.Range(startLine, startChar, endLine, endChar), options: { inlineClassName: cssClass } }
    ];

    const decorations = myEditor.editor.deltaDecorations(oldDecorations, newDecorations);
    myEditor.rangeHighlightDecorationId = decorations[0];

  },


  ResetDecorations: function (id) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }

    if (myEditor.rangeHighlightDecorationId) {
      myEditor.editor.deltaDecorations([myEditor.rangeHighlightDecorationId], []);
      myEditor.rangeHighlightDecorationId = null;
    }

  },

  SetValue: function (id, value) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    myEditor.editor.setValue(value);
    return true;
  },
  SetTheme: function (id, theme) {
    let myEditor = window.Blazaco.Editors.find(e => e.id === id);
    if (!myEditor) {
      throw `Could not find a editor with id: '${window.Blazaco.Editors.length}' '${id}'`;
    }
    monaco.editor.setTheme(theme);
    return true;
  }
}