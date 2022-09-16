function enterKey() {
    $('#repairlistrp input, .withtabindex').bind("keydown", function (e) {
        if (e.keyCode == 13) {
            const controls = $('#repairlistrp .withtabindex').filter((index, curr) => !curr.readOnly && !curr.disabled);
            const tabControls = controls.map((index, curr) => {
                if (curr.tagName === 'DIV') {
                    return curr.getElementsByTagName('input')[0];
                } else {
                    return curr;
                }
            });
            const idx = tabControls.index(this);
            if (idx == tabControls.length - 1) {
                if (this.tagName === 'BUTTON') {
                    return true;
                } else {
                    tabControls[0].focus();
                }
            } else {
                if (this.tagName === 'A' && !this.classList.contains('active')) {
                    return true;
                } else {
                    tabControls[idx + 1].focus();
                }
            }
            return false;
        }
    });
}
function tabKey() {
    $('#repairlistrp input, .withtabindex').bind("keydown", function (e) {
        if (e.keyCode == 9 && !e.shiftKey) {
            const controls = $('#repairlistrp .withtabindex').filter((index, curr) => !curr.readOnly && !curr.disabled);
            const tabControls = controls.map((index, curr) => {
                if (curr.tagName === 'DIV') {
                    return curr.getElementsByTagName('input')[0];
                } else {
                    return curr;
                }
            });
            const idx = tabControls.index(this);
            if (idx == tabControls.length - 1) {
                tabControls[0].focus();
            } else {
                tabControls[idx + 1].focus();
            }
            return false;
        }
        if (e.keyCode == 9 && e.shiftKey) {
            const controls = $('#repairlistrp .withtabindex').filter((index, curr) => !curr.readOnly && !curr.disabled);
            const tabControls = controls.map((index, curr) => {
                if (curr.tagName === 'DIV') {
                    return curr.getElementsByTagName('input')[0];
                } else {
                    return curr;
                }
            });
            const idx = tabControls.index(this);
            if (idx == 0) {
                tabControls[tabControls.length - 1].focus();
            } else {
                tabControls[idx - 1].focus();
            }
            return false;
        }
    })
}