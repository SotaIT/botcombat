function Game(model, canvasId, logId, rangeId, speedId, playStopId, progressId) {

    // init variables
    const that = this;
    const map = model.Game.Map;
    const steps = model.Game.Steps;
    const scale = model.Scale;
    const objectSize = model.Scale;
    const width = map.Width * scale;
    const height = map.Height * scale;
    const bulletSpeed = model.BulletSpeed;
    const frameCount = 8;

    const images = {};
    const state = {
        stepNumber: 0,
        frameNumber: 0,
        stopRequested: false,
        isPlaying: false
    };

    let speed = 1 / frameCount;
    let frameInterval = speed * scale / frameCount;
    let loadingImageCount = 0;

    // ui
    const canvas = $(canvasId);
    canvas.attr('width', width);
    canvas.attr('height', height);

    const log = $(logId);

    const rangeControl = $(rangeId);
    rangeControl.attr('min', 0);
    rangeControl.attr('max', steps.length - 1);
    rangeControl.prop('value', 0);
    rangeControl.on('change', function () {
        that.setCurrentStep(parseInt(rangeControl.val()));
    });

    const speedControl = $(speedId);
    speedControl.on('change', function () {
        that.setSpeed(parseInt(speedControl.val()));
    });

    const playIcon = '&#x25B6;';//'&#9205;';
    const pauseIcon = '&#9208;';//'&#9724;';
    const playStop = $(playStopId);
    playStop.html(pauseIcon);
    playStop.on('click', function () {

        if (state.isPlaying) {
            that.pause();
            playStop.html(playIcon);
        } else {
            that.resume();
            playStop.html(pauseIcon);
        }
    });
    const progressText = $(progressId);

    // functions
    function setStepNumber(n) {
        state.stepNumber = n;

        progressText.html(n + ' / ' + (steps.length - 1));
    }

    function setFrameNumber(n) {
        state.frameNumber = n;
    }

    function changeSpeed(n) {
        speed = 1 / n;
        frameInterval = speed * scale / frameCount;
    }

    function preLoadImages() {
        for (let im in model.Images)
            if (model.Images.hasOwnProperty(im)) {
                let img = new Image();
                img.src = model.Images[im];
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

    function getBotName(id) {
        return model.BotNames[id];
    }

    function getBulletImage(id) {
        return images[model.BulletImages[id]];
    }

    function getShotImage(id) {
        return images[model.ShotImages[id]];
    }

    function getExplosionImage(id) {
        return images[model.ExplosionImages[id]];
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
        let f = Math.floor(state.frameNumber * speed);
        while (f > frameCount - 1)
            f -= frameCount;
        return objectSize * f + objectSize;
    }

    function getPrevBot(id) {
        const stepNum = state.stepNumber;
        if (stepNum > 0)
            for (let j = 0; j < steps[stepNum - 1].Bt.length; j++) {
                const prev = steps[stepNum - 1].Bt[j];
                if (prev.Id === id) {
                    return prev;
                }
            }
        return null;
    }

    function getDirOffset(dr) {
        switch (dr) {
            // up
            case 1:
                return 0;
            // right
            case 2:
                return objectSize;
            // down
            case 3:
                return objectSize * 2;
            // left
            case 4:
                return objectSize * 3;
            // none
            default:
                return -1;
        }
    }

    function getBotMoveOffset(x1, x2) {
        return x1 * scale + (x2 - x1) * state.frameNumber;
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
                if (bt.X === prevBot.X && bt.Y === prevBot.Y)
                    sx = 0;

                x = getBotMoveOffset(prevBot.X, bt.X);
                y = getBotMoveOffset(prevBot.Y, bt.Y);
            }

            ctx.drawImage(getBotImage(bt.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function getPrevBullet(id, n) {
        const stepNum = state.stepNumber;
        if (stepNum > 0)
            for (let j = 0; j < steps[stepNum - 1].Bl.length; j++) {
                const prev = steps[stepNum - 1].Bl[j];
                if (prev.Id === id && prev.N === n) {
                    return prev;
                }
            }
        return null;
    }

    function getBulletMoveOffset(prev, real, next) {
        if (next === prev) return prev * scale;

        const up = next > real;
        const down = next < real;
        let diff = real - prev;

        if (up || down) {

            const realScaled = real * scale;
            if (up)
                diff++;
            else
                diff--;
            const newReal = prev * scale + diff * state.frameNumber;
            return newReal > realScaled && up || newReal < realScaled && !up
                ? -1
                : newReal;
        }
        return prev * scale + diff * state.frameNumber;
    }

    function getNewPos(dr, x, y, itemSpeed) {
        switch (dr) {
            // up
            case 1:
                return { X: x, Y: y - itemSpeed };
            // right
            case 2:
                return { X: x + itemSpeed, Y: y };
            // down
            case 3:
                return { X: x, Y: y + itemSpeed };
            // left
            case 4:
                return { X: x - itemSpeed, Y: y };
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
            let show = true;

            let prev = getPrevBullet(bl.Id, bl.N);
            if (prev === null)
                prev = getPrevBot(bl.Id);
            if (prev) {
                sy = getDirOffset(bl.Dr);
                sx = computeSx();
                const next = getNewPos(bl.Dr, prev.X, prev.Y, bulletSpeed);
                x = getBulletMoveOffset(prev.X, bl.X, next.X);
                y = getBulletMoveOffset(prev.Y, bl.Y, next.Y);
                show = x !== -1 && y !== -1;
            }

            if (show)
                ctx.drawImage(getBulletImage(bl.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function drawShots(ctx, step) {

        for (let i = 0; i < step.Ss.length; i++) {
            const s = step.Ss[i];
            const next = getNewPos(s.Dr, s.X, s.Y, 1);
            const x = next.X * scale,
                y = next.Y * scale,
                sy = getDirOffset(s.Dr),
                sx = computeSx();
            ctx.drawImage(getShotImage(s.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
        }
    }

    function drawExplosions(ctx) {
        //draw explosions of previous step in current step
        const stepNum = state.stepNumber;
        if (stepNum <= 0) return;
        const st = steps[stepNum - 1];

        for (let i = 0; i < st.Es.length; i++) {
            const e = st.Es[i];
            const x = e.X * scale,
                y = e.Y * scale,
                sy = 0,
                sx = computeSx();
            ctx.drawImage(getExplosionImage(e.Id), sx, sy, objectSize, objectSize, x, y, scale, scale);
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
        let bg = getBgImage();
        ctx.drawImage(bg, 0, 0, width, height);
    }

    function startAnimation() {
        if (state.isPlaying) return;
        state.isPlaying = true;
        window.requestAnimationFrame(draw);
    }

    function stopAnimation() {
        if (!state.isPlaying) return;
        state.isPlaying = false;
        state.stopRequested = true;
    }

    function draw() {

        if (state.stopRequested) {
            state.stopRequested = false;
            return;
        }

        if (steps.length <= 0) {
            window.requestAnimationFrame(draw);
            stopAnimation();
            return;
        }
        const stepNum = state.stepNumber;
        const step = steps[stepNum];

        const ctx = canvas[0].getContext('2d');
        ctx.globalCompositeOperation = 'destination-over';
        ctx.clearRect(0, 0, width, height);

        drawShots(ctx, step);
        drawExplosions(ctx);
        drawWalls(ctx);
        drawBots(ctx, step);
        drawBullets(ctx, step);
        drawBonuses(ctx, step);
        drawTraps(ctx);
        drawBackground(ctx);

        setFrameNumber(state.frameNumber + frameInterval);
        if (state.frameNumber >= scale) {
            writeLogs(step, stepNum);
            rangeControl.prop('value', stepNum);
            if (stepNum >= steps.length - 1) {
                stopAnimation();
            } else {
                setStepNumber(stepNum + 1);
                setFrameNumber(0);
            }
        }

        window.requestAnimationFrame(draw);
    }

    function getBotNameForLog(id) {
        return '<b>' + getBotName(id) + '</b>';
    }

    function writeLogs(step, stepNum) {
        for (let i = 0; i < step.L.length; i++) {
            const l = step.L[i];
            let msg = '';
            let color = 'black';

            switch (l.T) {
                case 1:
                    msg = getBotNameForLog(l.Si) + ' made ' + l.V + ' damage to ' + getBotNameForLog(l.Ti) + ' at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 2:
                    msg = getBotNameForLog(l.Ti) + ' got ' + l.V + ' power from bonus at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 3:
                    msg = getBotNameForLog(l.Ti) + ' took ' + l.V + ' damage from trap at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 4:
                    msg = getBotNameForLog(l.Ti) + ' was killed at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 5:
                    msg = getBotNameForLog(l.Si) + ' made ' + l.V + ' ranged damage to ' + getBotNameForLog(l.Ti) + ' at (' + l.X + ', ' + l.Y + ')';
                    break;
                case 6:
                    msg = (l.Ti ? getBotNameForLog(l.Ti) + ' > ' : '') + l.M;
                    color = 'orange';
                    break;
                case 7:
                    msg = (l.Ti ? getBotNameForLog(l.Ti) + ' > ' : '') + l.M;
                    color = 'red';
                    break;
            }

            writeLog('<div style="color: ' + color + '">Step ' + stepNum + ': ' + msg + '</div>');

            //if (step.bots && stepNumber === steps.length)
            //    for (let i = 0; i < step.bots.length - 1; i++)
            //        if (step.bots[i].H > 0)
            //            writeLog('Step ' + (stepNumber - 1) + ': Bot #' + step.bots[i].id + ' won!');

            if (stepNum >= steps.length - 1)
                writeLog('Game Over!');
        }
    }

    function writeLog(s) {
        log.html(s + log.html());
    }

    function reWriteLogs() {
        log.html('');
        for (let i = 0; i < steps.length && i <= state.stepNumber; i++)
            writeLogs(steps[i], i);
    }

    // public functions
    this.play = function () {
        while (loadingImageCount > 0);

        startAnimation();
    };

    this.pause = function () {
        stopAnimation();
    };

    this.resume = function () {
        startAnimation();
    };

    this.setCurrentStep = function (n) {
        if (!n || n < 0) n = 0;
        stopAnimation();
        setStepNumber(n);
        setFrameNumber(0);
        reWriteLogs(n);
        startAnimation();
    };

    this.setSpeed = function (n) {
        stopAnimation();
        changeSpeed(n);
        writeLog('speed: ' + speed);
        startAnimation();
    };

    // init
    function init() {
        changeSpeed(8);
        preLoadImages();
    }

    init();

}