//Ajax call to get championship teams.
function LoadTeams(championshipId) {
    $.ajax({
        url: "/Championship/GetChampionshipTeams",
        method: "get",
        data: { id: championshipId },
        success: function (teamList) {
            GetTeamsSuccess(teamList, championshipId);
        }
    });
}

//Function called after LoadTeams ajax success.
async function GetTeamsSuccess(teamList, championshipId) {
    var totalTeams = teamList.length;
    var totalNodes = (totalTeams * 2) - 2; //The total nodes of a binary tree is equal to (number of leaves * 2) - 2 (Zero Based)

    //Awaits the brackets generation before filling them.
    await GenerateBrackets(totalTeams, totalNodes);
    await FillBrackets(teamList);

    //After brackets are filled.
    FillComplete(championshipId);
}

//Ajax call to get the DynBrackets view, which generates the brackets based on the total of nodes and number of teams.
async function GenerateBrackets(totalTeams, totalNodes) {
    await $.ajax({
        url: "/Championship/GenerateBrackets",
        method: "get",
        data: {
            numberOfTeams: totalTeams, totalNodes: totalNodes
        },
        success: function (data) {
            $("#renderedBrackets").html(data);
        }
    });
}

//Place every team in their respective brackets based on the tree position(index).
async function FillBrackets(teamList) {
    await teamList.forEach((value) => {
        //Rectangle
        $("#" + value.treePosition).html(value.name);

        var btnFrwd = $("#btnFrwd" + value.treePosition);

        //If the team is out, there's no need to have a forward button to it.
        if (!value.active)
            btnFrwd.remove();
        else
            btnFrwd.data("teamid", value.id); //Adds the team id as a HTML5 data property.
    });
}

//Declares important functions and events after the filling process.
function FillComplete(championshipId) {
    $(".btn-frwd").on("click", function () {
        var teamId = $(this).data("teamid");

        //Ajax call to advance the team to the next bracket(phase)
        $.ajax({
            url: "/Championship/AdvanceToNextPhase",
            method: "post",
            data: { id: championshipId, teamId },
            success: function () {
                //Reload the page
                location.reload();
            }
        });
    });

    //Function which loops through all rectangle class elements.
    $(".rectangle").each(function () {
        var rectangle = this;
        var $rectangle = $(rectangle);
        var siblingRect = $rectangle.siblings(".rectangle");

        //If the rectangle or it's sibling rectangle is empty, removes the forward button that has the same id(index).
        if ($rectangle.html() === "" || siblingRect.html() === "")
        {
            var btn = $("#btnFrwd" + rectangle.id);
            btn.remove();
        }        
    });
}