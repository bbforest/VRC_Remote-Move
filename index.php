<title>VRChat Remote Move</title>
<form name="send" method="post">
<form>
    <table border=0 bgcolor="55B3F2" align=center width=700>
        <tr>
            <td align=center colspan=2>
                <font size="4" color="white">
                    <b>VRChat Remote Move</b>
                </font>
            </td>
        </tr>
        <tr bgcolor=white>
            <td align=right>
                PC : 
            </td>
            <td>
                <input type="text" name="PC" style="width:100%;">
            </td>
        </tr>
        <tr bgcolor=white>
            <td align=right>
                CMD : 
            </td>
            <td>
                <input type="text" name="CMD" style="width:100%;">
            </td>
        </tr>
        <tr bgcolor=white>
            <td align=center colspan=2>
                <input type="submit" value="전송" name="Submit" style="width:10%;">
                <small>
                    <span>관리자 외 접속금지</span>
                </small>
            </td>
        </tr>
    </table>
</form>
<?php
$fp = fopen($_POST[PC], "w");
fwrite($fp, $_POST[CMD]); 
fclose($fp); 
?>