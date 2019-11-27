function Game(model, canvasId, logId) {

    // init variables
    const map = model.Game.Map;
    const steps = model.Game.Steps;
    const scale = model.Scale;
    const width = map.Width * scale;
    const height = map.Height * scale;
    const bulletSpeed = model.BulletSpeed;

    const canvas = $(canvasId);
    const log = $(logId)[0];

    canvas.attr('width', width);
    canvas.attr('height', height);

    const speed = 1 / 2 / 2 / 2;
    const frameInterval = speed * scale / 8;
    const objectSize = scale;
    let stepNumber = 0;
    let frameNumber = 0;

    const images = {};
    const bulletImages = {};
    let loadingImageCount = 0;


    // functions

    function preLoadImages() {
        for (let im in model.Images)
            if (model.Images.hasOwnProperty(im)) {
                let img = new Image();
                img.src = '/img/' + model.Images[im];
                img.onload = imageLoaded();
                images[im] = img;
                loadingImageCount++;
            }
    }

    function imageLoaded() {
        loadingImageCount--;
    }

    function getBotImage(id) {
        return images[model.BotImages[id]];
    }

    function getBulletImage(id) {
        if (!bulletImages.hasOwnProperty(id)) {
            let img = new Image();
            img.src = images[model.BotImages[id]].src.replace('/img/bot/', '/img/bot/bullet-');
            bulletImages[id] = img;
        }
        return bulletImages[id];
    }

    function getBonusImage(id) {
        return images[model.BonusImages[id]];
    }

    function getTrapImage(id) {
        return images[model.TrapImages[id]];
    }

    function getWallImage(id) {
        return images[model.WallImages[id]];
    }

    function getBgImage() {
        return images[model.Background];
    }

    function computeSx() {
        let f = Math.floor(8 * 8 * frameNumber * speed / scale);
        while (f > 7)
            f -= 8;
        return objectSize * f;
    }

    function getPrevBot(id) {
        if (stepNumber > 0)
            for (let j = 0; j < steps[stepNumber - 1].Bt.length; j++) {
                const prev = steps[stepNumber - 1].Bt[j];
                if (prev.Id === id) {
                    return prev;
                }
            }
        return null;
    }

    function getDirOffset(dr) {
        switch (dr) {
            // up
            case 2:
                return objectSize;
            // right
            case 3:
                return objectSize * 4;
            // down
            case 4:
                return objectSize * 3;
            // left
            case 5:
                return objectSize * 2;
            // none
            default:
                return 0;
        }
    }

    function getBotMoveOffset(x1, x2) {
        return x1 * scale + (x2 - x1) * frameNumber;
    }

    function drawBots(ctx, step) {

        for (let i = 0; i < step.Bt.length; i++) {
            const bt = step.Bt[i];
            let x = bt.X * scale, y = bt.Y * scale;
            let sx = 0, sy = 0;
            const prevBot = getPrevBot(bt.Id);
            if (prevBot) {
                sx = computeSx();
                sy = getDirOffset(bt.Dr);
                if (sy === 0)
                    sx = 0;

                x = getBotMoveOffset(prevBot.X, bt.X);
                y = getBotMoveOffset(prevBot.Y, bt.Y);
            }

            ctx.drawImage(getBotImage(bt.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function getPrevBullet(id, n) {
        if (stepNumber > 0)
            for (let j = 0; j < steps[stepNumber - 1].Bl.length; j++) {
                const prev = steps[stepNumber - 1].Bl[j];
                if (prev.Id === id && prev.N === n) {
                    return prev;
                }
            }
        return null;
    }

    function getBulletMoveOffset(x1, x2, x3) {
        if (x3 === x1) return x1 * scale;
        let diff = x2 - x1;
        if (x3 > x2)
            diff++;
        else if (x3 < x2)
            diff--;
        return x1 * scale + diff * frameNumber;
    }

    function getNewPos(dr, x, y) {
        switch (dr) {
            // up
            case 2:
                return { X: x, Y: y - bulletSpeed };
            // right
            case 3:
                return { X: x + bulletSpeed, Y: y };
            // down
            case 4:
                return { X: x, Y: y + bulletSpeed };
            // left
            case 5:
                return { X: x - bulletSpeed, Y: y };
            // none
            default:
                return { X: x, Y: y };
        }
    }

    function drawBullets(ctx, step) {

        for (let i = 0; i < step.Bl.length; i++) {
            const bl = step.Bl[i];
            let x = bl.X * scale, y = bl.Y * scale;
            let sx = 0, sy = 0;

            let prev = getPrevBullet(bl.Id, bl.N);
            if (prev === null)
                prev = getPrevBot(bl.Id);
            if (prev) {
                sy = getDirOffset(bl.Dr);
                sx = computeSx();
                const next = getNewPos(bl.Dr, prev.X, prev.Y);
                x = getBulletMoveOffset(prev.X, bl.X, next.X);
                y = getBulletMoveOffset(prev.Y, bl.Y, next.Y);
            }

            ctx.drawImage(getBulletImage(step.Bl[i].Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function drawBonuses(ctx, step) {
        for (let i = 0; i < step.Bs.length; i++) {
            const b = step.Bs[i];
            ctx.drawImage(getBonusImage(b.Id), b.X * scale, b.Y * scale, scale, scale);
        }
    }

    function drawTraps(ctx) {
        for (let i = 0; i < map.Traps.length; i++) {
            const t = map.Traps[i];
            ctx.drawImage(getTrapImage(t.Id), t.X * scale, t.Y * scale, scale, scale);
        }
    }

    function drawWalls(ctx) {
        for (let i = 0; i < map.Walls.length; i++) {
            const w = map.Walls[i];
            ctx.drawImage(getWallImage(w.Id), w.X * scale, w.Y * scale, scale, scale);
        }
    }

    function drawBackground(ctx) {
        ctx.drawImage(getBgImage(), 0, 0, map.width, map.height);
    }

    function startAnimation() {
        window.requestAnimationFrame(draw);
    }

    function draw() {

        if (steps.length === 0) {
            window.requestAnimationFrame(draw);
            return;
        }

        const step = steps[stepNumber];

        const ctx = canvas[0].getContext('2d');
        ctx.globalCompositeOperation = 'destination-over';
        ctx.clearRect(0, 0, width, height);

        drawWalls(ctx);
        drawBots(ctx, step);
        drawBullets(ctx, step);
        drawBonuses(ctx, step);
        drawTraps(ctx);
        drawBackground(ctx);

        frameNumber += frameInterval;
        if (frameNumber >= scale) {
            writeLogs(step);
            stepNumber++;
            frameNumber = 0;
        }

        if (stepNumber >= steps.length) {
            if (step.bots.length > 0)
                writeLog('Step ' + (stepNumber - 1) + ': Bot #' + step.bots[0].id + ' won!');
            writeLog('Game Over!');
            return;
        }

        window.requestAnimationFrame(draw);
    }

    function writeLogs(step) {
        for (let i = 0; i < step.L.length; i++) {
            const l = step.L[i];
            let msg = '';

            switch (l.T) {
                case 1:
                    msg = 'Bot #' + l.Si + ' made ' + l.V + ' damage to bot #' + l.Ti + ' at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 2:
                    msg = 'Bot #' + l.Ti + ' got ' + l.V + ' power from bonus at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 3:
                    msg = 'Bot #' + l.Ti + ' took ' + l.V + ' damage from trap at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 4:
                    msg = 'Bot #' + l.Ti + ' was killed at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 5:
                    msg = 'Bot #' + l.Si + ' made ' + l.V + ' ranged damage to bot #' + l.Ti + ' at (' + l.X + ', ' + l.Y + ')';
                    break;
            }

            writeLog('Step ' + stepNumber + ': ' + msg);
        }
    }

    function writeLog(s) {
        let record = document.createElement('div');
        record.innerHTML = s;
        log.insertBefore(record, log.firstChild);
    }

    // public functions
    this.play = function () {
        while (loadingImageCount > 0);

        startAnimation();
    };

    // init
    function init() {
        preLoadImages();
    }

    init();

}