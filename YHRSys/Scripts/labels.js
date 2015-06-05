var Barcode25x13mmXml = '<?xml version="1.0" encoding="utf-8"?>\
        <DieCutLabel Version="8.0" Units="twips">\
	        <PaperOrientation>Landscape</PaperOrientation>\
	        <Id>Small30333</Id>\
	        <PaperName>30333 1/2 in x 1 in (2 up)</PaperName>\
	        <DrawCommands>\
		        <Path>\
			        <FillMode>EvenOdd</FillMode>\
			        <RoundRectangle X="0" Y="0" Width="720" Height="1440" Rx="180" Ry="180" />\
			        <RoundRectangle X="720" Y="0" Width="720" Height="1440" Rx="180" Ry="180" />\
		        </Path>\
	        </DrawCommands>\
	        <ObjectInfo>\
		        <BarcodeObject>\
			        <Name>topLabel</Name>\
			        <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			        <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			        <LinkedObjectName></LinkedObjectName>\
			        <Rotation>Rotation0</Rotation>\
			        <IsMirrored>False</IsMirrored>\
			        <IsVariable>False</IsVariable>\
			        <Text>1234</Text>\
			        <Type>Code39</Type>\
			        <Size>Small</Size>\
			        <TextPosition>None</TextPosition>\
			        <TextFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			        <CheckSumFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			        <TextEmbedding>None</TextEmbedding>\
			        <ECLevel>0</ECLevel>\
			        <HorizontalAlignment>Center</HorizontalAlignment>\
			        <QuietZonesPadding Left="0" Top="0" Right="0" Bottom="0" />\
		        </BarcodeObject>\
		        <Bounds X="134" Y="57" Width="1219" Height="374.173248291016" />\
	        </ObjectInfo>\
	        <ObjectInfo>\
		        <BarcodeObject>\
			        <Name>bottomLabel</Name>\
			        <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			        <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			        <LinkedObjectName></LinkedObjectName>\
			        <Rotation>Rotation0</Rotation>\
			        <IsMirrored>False</IsMirrored>\
			        <IsVariable>False</IsVariable>\
			        <Text>1234</Text>\
			        <Type>Code39</Type>\
			        <Size>Small</Size>\
			        <TextPosition>None</TextPosition>\
			        <TextFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			        <CheckSumFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			        <TextEmbedding>None</TextEmbedding>\
			        <ECLevel>0</ECLevel>\
			        <HorizontalAlignment>Center</HorizontalAlignment>\
			        <QuietZonesPadding Left="0" Top="0" Right="0" Bottom="0" />\
		        </BarcodeObject>\
		        <Bounds X="134" Y="803" Width="1219" Height="374.173248291016" />\
	        </ObjectInfo>\
	        <ObjectInfo>\
		        <TextObject>\
			        <Name>topText</Name>\
			        <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			        <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			        <LinkedObjectName></LinkedObjectName>\
			        <Rotation>Rotation0</Rotation>\
			        <IsMirrored>False</IsMirrored>\
			        <IsVariable>False</IsVariable>\
			        <HorizontalAlignment>Center</HorizontalAlignment>\
			        <VerticalAlignment>Top</VerticalAlignment>\
			        <TextFitMode>ShrinkToFit</TextFitMode>\
			        <UseFullFontHeight>True</UseFullFontHeight>\
			        <Verticalized>False</Verticalized>\
			        <StyledText />\
		        </TextObject>\
		        <Bounds X="239" Y="435" Width="1024" Height="180" />\
	        </ObjectInfo>\
	        <ObjectInfo>\
		        <TextObject>\
			        <Name>bottomText</Name>\
			        <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			        <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			        <LinkedObjectName></LinkedObjectName>\
			        <Rotation>Rotation0</Rotation>\
			        <IsMirrored>False</IsMirrored>\
			        <IsVariable>False</IsVariable>\
			        <HorizontalAlignment>Center</HorizontalAlignment>\
			        <VerticalAlignment>Top</VerticalAlignment>\
			        <TextFitMode>ShrinkToFit</TextFitMode>\
			        <UseFullFontHeight>True</UseFullFontHeight>\
			        <Verticalized>False</Verticalized>\
			        <StyledText />\
		        </TextObject>\
		        <Bounds X="209" Y="1178" Width="1069" Height="180" />\
	        </ObjectInfo>\
        </DieCutLabel>';

var Barcode51x19mmXml = '<?xml version="1.0" encoding="utf-8"?>\
<DieCutLabel Version="8.0" Units="twips">\
	<PaperOrientation>Landscape</PaperOrientation>\
	<Id>MultiPurpose11355</Id>\
	<PaperName>11355 Multi-Purpose</PaperName>\
	<DrawCommands>\
		<RoundRectangle X="0" Y="0" Width="1080" Height="2880" Rx="180" Ry="180" />\
	</DrawCommands>\
	<ObjectInfo>\
		<BarcodeObject>\
			<Name>Barcode</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<Text>OGOJA12345</Text>\
			<Type>Code39</Type>\
			<Size>Small</Size>\
			<TextPosition>Bottom</TextPosition>\
			<TextFont Family="Arial" Size="10" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			<CheckSumFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			<TextEmbedding>None</TextEmbedding>\
			<ECLevel>0</ECLevel>\
			<HorizontalAlignment>Center</HorizontalAlignment>\
			<QuietZonesPadding Left="0" Top="0" Right="0" Bottom="0" />\
		</BarcodeObject>\
		<Bounds X="326" Y="158" Width="2413" Height="720" />\
	</ObjectInfo>\
</DieCutLabel>';

function Padder(len, pad) {
    if (len === undefined) {
        len = 1;
    } else if (pad === undefined) {
        pad = '0';
    }

    var pads = '';
    while (pads.length < len) {
        pads += pad;
    }

    this.pad = function (what) {
        var s = what.toString();
        return pads.substring(0, pads.length - s.length) + s;
    };
}

function loadPrinters() {
    var printers = dymo.label.framework.getPrinters();
    if (printers.length == 0) {
        alert("No DYMO printers are installed. Install DYMO printers.");
        return false;
    }
    return true;
}

function printLocally() {
    var copies = $("#copies").val();
    var labeltype = $("#labeltype").val();

    var RecordId = 0;
    if ($('#processId').attr('value'))
        RecordId = $('#processId').val();

    var SearchString = "";
    var SearchStartProcessDate = "";
    var SearchEndProcessDate = "";

    if ($('#SearchString').attr('value'))
        SearchString = $('#SearchString').val();

    if ($('#SearchStartProcessDate').attr('value'))
        SearchStartProcessDate = $('#SearchStartProcessDate').val();

    if ($('#SearchEndProcessDate').attr('value'))
        SearchEndProcessDate = $('#SearchEndProcessDate').val();

    var zero4 = new Padder(4);

    if (labeltype == "Barcode25x13mm")
        var label = dymo.label.framework.openLabelXml(Barcode25x13mmXml);
    else
        var label = dymo.label.framework.openLabelXml(Barcode51x19mmXml);

    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/VarietyProcessFlow/IndexDataList",
        data: { searchString: SearchString, searchStartProcessDate: SearchStartProcessDate, searchEndProcessDate: SearchEndProcessDate, copies: copies, id: RecordId },
        success: function (data) {
            if (loadPrinters() == true) {
                try {
                    var count = Object.keys(data).length * 1;
                } catch (e) {
                    var count = 0;
                    for (var prop in data) {
                        if (data.hasOwnProperty(prop))
                            count++;
                    }
                }
                if (count > 0) {
                    //if (copies == "" || copies == null) copies = 1; else copies = copies * 1;

                    for (var i = 0; i < count; i++) {
                        if (labeltype == "Barcode25x13mm") {
                            //Check mod of i for us to write on the topLabel of the multilabel type
                            if ((i % 2) == 0) {
                                var vpfID = data[i].processId;
                                label.setObjectText("topLabel", zero4.pad(vpfID));
                                label.setObjectText("topText", data[i].barcode);
                            }
                            //Check if array index exist for us to write on the bottomLabel of the multilabel type
                            var k = (i + 1) * 1;
                            if (count > k) {
                                var vpfID2 = data[k].processId;
                                label.setObjectText("bottomLabel", zero4.pad(vpfID2));
                                label.setObjectText("bottomText", data[k].barcode);
                                i++;
                            }
                            else {
                                label.setObjectText("bottomLabel", "");
                                label.setObjectText("bottomText", "");
                            }
                        }
                        else {
                            label.setObjectText("Barcode", data[i].barcode);
                        }
                        //Try and catch any printer thrown errors
                        label.print("DYMO LabelWriter 4XL");
                    }
                }
            }
        },
        error: function (xhr, status, err) {
            alert("Error: " + xhr.responseText);
        }
    });
}