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

async function GetTeamsSuccess(teamList, championshipId) {
    var totalTeams = teamList.length;
    var totalNodes = (totalTeams * 2) - 2;

    await GenerateBrackets(totalTeams, totalNodes);
    await FillBrackets(teamList);
    FillComplete(championshipId);
}

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

async function FillBrackets(teamList) {
    await teamList.forEach((value) => {
        //Rectangle
        $("#" + value.treePosition).html(value.name);

        var btnFrwd = $("#btnFrwd" + value.treePosition);

        if (!value.active)
            btnFrwd.remove();
        else
            btnFrwd.data("teamid", value.id);
    });
}

function FillComplete(championshipId) {
    $(".btn-frwd").on("click", function () {
        var teamId = $(this).data("teamid");

        $.ajax({
            url: "/Championship/AdvanceToNextPhase",
            method: "post",
            data: { id: championshipId, teamId },
            success: function () {
                location.reload();
            }
        });
    });

    $(".rectangle").each(function () {
        var rectangle = this;
        var $rectangle = $(rectangle);
        var siblingRect = $rectangle.siblings(".rectangle");

        var btn = $("#btnFrwd" + rectangle.id);

        if ($rectangle.html() === "" || siblingRect.html() === "")
            btn.remove();
    });
}