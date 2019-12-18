var WebApp = function () {
    var ws;
    if ("WebSocket" in window) {

        // Let us open a web socket
        ws = new WebSocket("ws://" + location.hostname + ":3030", "ws-SFX-protocol");

        //ws.onopen = function () {
        //    // Web Socket is connected, send data using send()
        //    ws.send("<command>Play</command>");
        //    alert("Message is sent...");
        //};

        ws.onmessage = function (evt) {
            var received_msg = evt.data;
            //console.log("Message received:\n" + received_msg);
            BuildXMLFromString(received_msg);
            //document.getElementById("PrevMainText").innerHTML = xmlDoc.getElementsByTagName("PrevMainText")[0].childNodes[0].nodeValue;
            var DisplaySettings = xmlDoc.getElementsByTagName("DisplaySettings")[0].childNodes;
            if (DisplaySettings != null) {
                for (i = 0; i < DisplaySettings.length; i++) {
                    if (DisplaySettings[i].nodeType == Node.ELEMENT_NODE) {
                        if (DisplaySettings[i + 1].nodeType == Node.TEXT_NODE) {
                            var field = document.getElementById(DisplaySettings[i].nodeName);
                            if (field != null) {
                                field.innerHTML = DisplaySettings[i].textContent;
                            } else {
                                console.log("Unable to locate id=" + DisplaySettings[i].nodeName + ". New value = " + DisplaySettings[i].textContent);
                            }
                        }
                    }
                }
            }
        };

        ws.onclose = function () {

            // websocket is closed.
            //alert("Connection is closed...");
        };
    } else {
        // The browser doesn't support WebSocket
        alert("WebSocket is not supported by your browser!");
    }
    this.sendCommand = function (command) {
        if (ws && ws.readyState == WebSocket.OPEN) {
            //ws.send("<root><command>" + command + "</command></root>");
            ws.send("<command>" + command + "</command>");
        }
    }
}

function init() {
    webapp = new WebApp();
}

document.addEventListener('DOMContentLoaded', init);

function CreateXMLDocument(str) {
    var xmlDoc = null;
    if (window.DOMParser) {
        var parser = new DOMParser();
        xmlDoc = parser.parseFromString(str, "text/xml");
    } else if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.loadXML(str);
    }
    return xmlDoc;
    //var customerNode = xmlDoc.getElementsByTagName("customer")[0];
    //var customerName = customerNode.getAttribute("name");
}

function CreateMSXMLDocumentObject() {
    if (typeof (ActiveXObject) != "undefined") {
        var progIDs = [
                        "Msxml2.DOMDocument.6.0",
                        "Msxml2.DOMDocument.5.0",
                        "Msxml2.DOMDocument.4.0",
                        "Msxml2.DOMDocument.3.0",
                        "MSXML2.DOMDocument",
                        "MSXML.DOMDocument"
        ];
        for (var i = 0; i < progIDs.length; i++) {
            try {
                return new ActiveXObject(progIDs[i]);
            } catch (e) { };
        }
    }
    return null;
}

function BuildXMLFromString(text) {
    var message = "";
    if (window.DOMParser) { // all browsers, except IE before version 9
        var parser = new DOMParser();
        try {
            xmlDoc = parser.parseFromString(text, "text/xml");
        } catch (e) {
            // if text is not well-formed,
            // it raises an exception in IE from version 9
            //alert("XML parsing error.");
            return false;
        };
    }
    else {  // Internet Explorer before version 9
        xmlDoc = CreateMSXMLDocumentObject();
        if (!xmlDoc) {
            alert("Cannot create XMLDocument object");
            return false;
        }

        xmlDoc.loadXML(text);
    }

    var errorMsg = null;
    if (xmlDoc.parseError && xmlDoc.parseError.errorCode != 0) {
        errorMsg = "XML Parsing Error: " + xmlDoc.parseError.reason
                  + " at line " + xmlDoc.parseError.line
                  + " at position " + xmlDoc.parseError.linepos;
    }
    else {
        if (xmlDoc.documentElement) {
            if (xmlDoc.documentElement.nodeName == "parsererror") {
                errorMsg = xmlDoc.documentElement.childNodes[0].nodeValue;
            }
        }
        else {
            errorMsg = "XML Parsing Error!";
        }
    }

    if (errorMsg) {
        alert(errorMsg);
        return false;
    }

    //alert("Parsing was successful!");
    return true;
}
